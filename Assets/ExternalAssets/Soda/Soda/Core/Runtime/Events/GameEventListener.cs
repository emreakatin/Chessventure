// Copyright © Sascha Graeff/13Pixels.

namespace ThirteenPixels.Soda
{
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// A MonoBehaviour listening to a specific GameEvent.
    /// Whenever the GameEvent is raised, the response UnityEvent is invoked.
    /// </summary>
    [HelpURL(SodaDocumentation.URL_RUNTIME + nameof(GameEventListener))]
    [AddComponentMenu("Soda/GameEvent Listener")]
    public class GameEventListener : MonoBehaviour
    {
#if UNITY_EDITOR
        internal static class PropertyNames
        {
            internal const string gameEvent = nameof(_gameEvent);
            internal const string deactivateAfterRaise = nameof(GameEventListener.deactivateAfterRaise);
            internal const string response = nameof(_response);
        }
#endif

        [Tooltip("The event to react upon.")]
        [SerializeField]
        private GameEventBase _gameEvent;
        /// <summary>
        /// The GameEvent to listen to.
        /// This can be changed at runtime - the response will be removed from the old GameEvent and added to the new one.
        /// </summary>
        public GameEventBase gameEvent
        {
            get => _gameEvent;
            set
            {
                if (_gameEvent == value) return;

                if (enabled && _gameEvent)
                {
                    _gameEvent.GetOnRaiseBase().RemoveListener(OnEventRaised);
                }

                _gameEvent = value;

                if (enabled && _gameEvent)
                {
                    _gameEvent.GetOnRaiseBase().AddListener(OnEventRaised);
                }
            }
        }

        [Tooltip("Deactivate this component after its GameEvent was raised.")]
        public bool deactivateAfterRaise = false;

        [Space]
        [Tooltip("Response to invoke when the event is raised.")]
        [SerializeField, UnityEngine.Serialization.FormerlySerializedAs("response")]
        private UnityEvent _response = default;
        /// <summary>
        /// The response to invoke when the referenced GameEvent is being raised.
        /// </summary>
        public UnityEvent response => _response;


        private void OnEnable()
        {
            if (gameEvent)
            {
                gameEvent.GetOnRaiseBase().AddListener(OnEventRaised);
            }
        }

        private void OnDisable()
        {
            if (gameEvent)
            {
                gameEvent.GetOnRaiseBase().RemoveListener(OnEventRaised);
            }
        }

        internal void OnEventRaised()
        {
            response.Invoke();

            if (deactivateAfterRaise)
            {
                enabled = false;
            }
        }
    }
}
