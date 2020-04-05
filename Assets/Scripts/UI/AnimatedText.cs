using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// The life bars displayed in the UI on top of its sources.
    /// </summary>    
    /// <seealso cref="IPoolableObject" />
    [RequireComponent(typeof(Text), typeof(Animation))]
    internal class AnimatedText : MonoBehaviour, IPoolableObject
    {
        [SerializeField] private Vector3 _showingPosition = Vector3.zero;
        private Text _text;
        private Animation _animation;

        /// <summary>
        /// Occurs when the attached Component is destroying as <see cref="IPoolableObject" />.
        /// </summary>
        public event Action<Component> OnDestroyAsPoolableObject;

        /// <summary>
        /// Gets or sets the prefab instance identifier.
        /// </summary>
        /// <value>The prefab instance identifier.</value>
        public int PrefabInstanceID { get; set; }

        /// <summary>
        /// Is called when the script instance is being loaded.
        /// </summary>        
        private void Awake()
        {
            _text = GetComponent<Text>();
            _animation = GetComponent<Animation>();
        }

        /// <summary>
        /// Setups the AnimatedText instance.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="parent">The parent transform.</param>
        public void Setup(string text, RectTransform parent)
        {
            SetText(text);
            transform.SetParent(parent);
            transform.localPosition = _showingPosition;
        }

        /// <summary>
        /// Returns this object to the object pool.
        /// </summary>
        public void DestroyAsPoolableObject()
        {
            OnDestroyAsPoolableObject?.Invoke(this);
        }

        /// <summary>
        /// Sets the text.
        /// </summary>
        /// <param name="text">The text.</param>
        public void SetText(string text) => _text.text = text;

        /// <summary>
        /// Plays the text animation.
        /// </summary>
        public void PlayAnimation() => _animation.Play();
    }
}
