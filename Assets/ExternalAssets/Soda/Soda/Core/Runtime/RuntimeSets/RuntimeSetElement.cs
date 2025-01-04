// Copyright © Sascha Graeff/13Pixels.

namespace ThirteenPixels.Soda
{
    using UnityEngine;

    /// <summary>
    /// A MonoBehaviour used to add a GameObject to a RuntimeSet.
    /// The element is only part of the RuntimeSet while it's active.
    /// </summary>
    [HelpURL(SodaDocumentation.URL_RUNTIME + nameof(RuntimeSetElement))]
    [AddComponentMenu("Soda/RuntimeSet Element")]
    public class RuntimeSetElement : MonoBehaviour
    {
#if UNITY_EDITOR
        internal static class PropertyNames
        {
            internal const string runtimeSet = nameof(_runtimeSet);
        }
#endif

        [Tooltip("The RuntimeSet to add this GameObject to.")]
        [SerializeField]
        [DisplayInsteadInPlaymode(nameof(runtimeSet))]
        private RuntimeSetBase _runtimeSet;
        /// <summary>
        /// The RuntimeSet to register with.
        /// This can be changed at runtime - the component will unregister from the previous RuntimeSet and register with the new one.
        /// </summary>
        public RuntimeSetBase runtimeSet
        {
            get
            {
                return _runtimeSet;
            }
            set
            {
                if (value == _runtimeSet) return;

                if (_runtimeSet && enabled)
                {
                    _runtimeSet.Remove(gameObject);
                }

                _runtimeSet = value;

                if (_runtimeSet && enabled)
                {
                    var couldAdd = _runtimeSet.Add(gameObject);
                    if (!couldAdd)
                    {
                        enabled = false;
                    }
                }
            }
        }

        private void OnEnable()
        {
            if (runtimeSet)
            {
                var couldAdd = runtimeSet.Add(gameObject);
                if (!couldAdd)
                {
                    enabled = false;
                }
            }
            else
            {
                Debug.LogError("This RuntimeSetElement does not have a RuntimeSet assigned.", this);
                enabled = false;
            }
        }

        private void OnDisable()
        {
            if (runtimeSet)
            {
                runtimeSet.Remove(gameObject);
            }
        }
    }
}
