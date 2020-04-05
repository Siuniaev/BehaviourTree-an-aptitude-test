using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// The base class for area-objects in game scene.
    /// </summary>    
    [RequireComponent(typeof(BoxCollider))]
    internal abstract class Area : MonoBehaviour
    {
        protected BoxCollider _collider;

        /// <summary>
        /// Is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            _collider = GetComponent<BoxCollider>();
        }

        /// <summary>
        /// Is called on the frame when a script is enabled just before any of the Update methods are called the first time.
        /// </summary>
        protected virtual void Start()
        {
            _collider.isTrigger = true;
        }
    }
}
