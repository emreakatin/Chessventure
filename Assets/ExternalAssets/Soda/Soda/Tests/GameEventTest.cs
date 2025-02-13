﻿// Copyright © Sascha Graeff/13Pixels.

namespace ThirteenPixels.Soda.Tests
{
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.TestTools;
    using NUnit.Framework;

    public class GameEventTest
    {
        private class GameEventInt : GameEventBase<int>
        {
            public class IntEvent : UnityEvent<int> { }
            public readonly IntEvent _onRaiseGlobally = new IntEvent();
            protected override UnityEvent<int> onRaiseGlobally => _onRaiseGlobally;
        }

        private GameEvent gameEvent;
        private GameEventInt gameEventInt;

        [SetUp]
        public void SetUp()
        {
            gameEvent = ScriptableObject.CreateInstance<GameEvent>();
            gameEventInt = ScriptableObject.CreateInstance<GameEventInt>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(gameEvent);
            Object.DestroyImmediate(gameEventInt);
        }

        [Test]
        public void Invocation()
        {
            var result = false;

            gameEvent.onRaise.AddListener(() => result = true);
            Assert.AreEqual(false, result);

            gameEvent.Raise();
            Assert.AreEqual(true, result);
        }

        [Test]
        public void ParameterizedInvocation()
        {
            var onRaiseWasRaised = false;
            var result = 0;

            gameEventInt.onRaise.AddListener(() => onRaiseWasRaised = true);
            gameEventInt.onRaise.AddListener(i => result = i);
            Assert.AreEqual(false, onRaiseWasRaised);
            Assert.AreEqual(0, result);

            gameEventInt.Raise(10);
            Assert.AreEqual(true, onRaiseWasRaised);
            Assert.AreEqual(10, result);
        }

        [Test]
        public void GlobalResponseBeforeSodaEventResponse()
        {
            var result = 0;

            gameEventInt.onRaise.AddListener(() => result = 2);
            gameEventInt._onRaiseGlobally.AddListener(i => result = 1);

            gameEventInt.Raise(0);

            Assert.AreEqual(2, result);
        }
    }
}
