// Copyright © Sascha Graeff/13Pixels.

namespace ThirteenPixels.Soda.Tests
{
    using UnityEngine.TestTools;
    using NUnit.Framework;
    using System;

    public class SodaEventTest
    {
        private SodaEvent sodaEvent;
        private SodaEvent<int> sodaEventInt;
        private int callbackValue;

        [SetUp]
        public void SetUp()
        {
            sodaEvent = new SodaEvent();
            sodaEventInt = new SodaEvent<int>(() => callbackValue);
            callbackValue = 0;
        }

        [Test]
        public void Invocation()
        {
            var result = false;

            sodaEvent.AddListener(() => result = true);
            Assert.AreEqual(false, result);

            sodaEvent.Invoke();
            Assert.AreEqual(true, result);
        }

        [Test]
        public void ParameterlessResponseSupport()
        {
            const int SOME_NUMBER = 42;
            var result = 0;

            Action response = () => result++;

            sodaEventInt.AddListener(response);
            Assert.AreEqual(0, result);

            sodaEventInt.Invoke(SOME_NUMBER);
            Assert.AreEqual(1, result);

            sodaEventInt.RemoveListener(response);

            sodaEventInt.Invoke(SOME_NUMBER);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void MultiInvocation()
        {
            var result = 0;
            sodaEvent.AddListener(() => result++);
            sodaEvent.AddListener(() => result += 2);
            sodaEvent.AddListener(() => result += 5);
            sodaEvent.Invoke();

            Assert.AreEqual(8, result);
        }

        [Test]
        public void ResponseRemoval()
        {
            var result = 0;
            Action response = () => result += 3;

            sodaEvent.AddListener(() => result += 2);
            sodaEvent.AddListener(response);
            sodaEvent.AddListener(() => result += 4);
            sodaEvent.Invoke();
            sodaEvent.RemoveListener(response);
            sodaEvent.Invoke();

            Assert.AreEqual(15, result);
        }

        [Test]
        public void ParameterInvocation()
        {
            var result = 0;
            sodaEventInt.AddListener(param => result += param);
            sodaEventInt.Invoke(5);
            sodaEventInt.Invoke(10);

            Assert.AreEqual(15, result);
        }

        [Test]
        public void AddResponseAndInvoke()
        {
            callbackValue = 10;
            var error = false;

            // This response should not be triggered by the upcoming AddResponseAndInvoke
            sodaEventInt.AddListener(i => error = true);

            sodaEventInt.AddResponseAndInvoke(param => callbackValue += param);

            Assert.AreEqual(20, callbackValue);
            Assert.AreEqual(false, error);
        }

        [Test]
        public void RecursiveInvocation()
        {
            var result = 0;
            sodaEvent.AddListener(() =>
            {
                result++;
                if (result < 10)
                {
                    sodaEvent.Invoke();
                }
            });
            sodaEvent.Invoke();

            Assert.AreEqual(10, result);
        }

        [Test]
        public void ExceptionInResponse()
        {
            var result = 0;
            sodaEvent.AddListener(() => result += 10);
            sodaEvent.AddListener(() => throw new SodaEventBase.TestException());
            sodaEvent.AddListener(() => result += 20);

            try
            {
                sodaEvent.Invoke();
            }
            catch
            {
                Assert.Fail();
            }

            Assert.AreEqual(30, result);
        }

        [Test]
        public void ResponseRemovalDuringInvocation()
        {
            var result = 0;

            Action plus20 = () => result += 20;

            sodaEvent.AddListener(() => result += 10);
            sodaEvent.AddListener(() => sodaEvent.RemoveListener(plus20));
            sodaEvent.AddListener(plus20);
            
            sodaEvent.Invoke();

            Assert.AreEqual(30, result);

            sodaEvent.Invoke();

            Assert.AreEqual(40, result);
        }
    }
}
