using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// The life bars displayed in the UI on top of its sources.
    /// </summary>    
    /// <seealso cref="IPoolableObject" />
    [RequireComponent(typeof(Image))]
    internal class HealthBar : MonoBehaviour, IPoolableObject
    {
        public const float FULLNESS_MIN = 0f;
        public const float FULLNESS_MAX = 1f;
        public const float POSITION_LERP_SPEED = 20f;

        [SerializeField] private Image _healthImage;
        [SerializeField] private TextJumper _textJumper;
        private float _heightOffset;
        private RectTransform _rectTransform;
        private RectTransform _parentRectTransform;
        private IShowableHealth _showable;
        private Camera _cameraMain;

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
            _rectTransform = GetComponent<RectTransform>();

            if (_healthImage == null)
                throw new UnityException($"There is no {nameof(_healthImage)} in {name}");
        }

        private void Start() => _textJumper?.UpdateText("");

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update() => UpdatePosition();

        /// <summary>
        /// Setups the specified target healthed object.
        /// </summary>
        /// <param name="healthedTarget">The target showable object.</param>
        /// <param name="parent">The UI transform that will be parent for this healthbar.</param>
        /// <param name="cameraMain">The camera main.</param>
        /// <exception cref="ArgumentNullException">
        /// healthedTarget
        /// or
        /// parentRectTransform
        /// or
        /// cameraMain
        /// </exception>
        public void Setup(IShowableHealth healthedTarget, RectTransform parent, Camera cameraMain)
        {
            _showable = healthedTarget ?? throw new ArgumentNullException(nameof(healthedTarget));
            _parentRectTransform = parent ?? throw new ArgumentNullException(nameof(parent));
            _cameraMain = cameraMain ?? throw new ArgumentNullException(nameof(cameraMain));
            _heightOffset = healthedTarget.HealthBarHeightOffset;
            transform.SetParent(_parentRectTransform);
            transform.localScale = Vector3.one;
            UpdatePosition(smooth: false);
            _rectTransform.gameObject.SetActive(true);
            SubscribeForHealthChanges();
            SetNewSizeDelta(healthedTarget.HealthBarWidth, _rectTransform.sizeDelta.y);
            _healthImage.fillAmount = FULLNESS_MAX;
        }

        /// <summary>
        /// Returns this object to the object pool.
        /// </summary>
        public void DestroyAsPoolableObject()
        {
            UnsubscribeForHealthChanges();
            OnDestroyAsPoolableObject?.Invoke(this);
        }

        /// <summary>
        /// Called when the health has changed.
        /// </summary>
        /// <param name="changedHealthArgs">The changed health arguments.</param>
        public void OnHealthChangesHandler(ChangedHealthArgs changedHealthArgs)
        {
            _healthImage.fillAmount = changedHealthArgs.Fullness;
            _textJumper?.UpdateText(changedHealthArgs.Description);
        }

        /// <summary>
        /// Colled when the showable does not need to be shown.
        /// </summary>
        /// <param name="showable">The showable.</param>
        public void OnHealthStopShowingHandler(IShowableHealth showable)
        {
            if (_showable != showable)
                return;

            _showable.OnHealthStopShowing -= OnHealthStopShowingHandler;
            _showable.OnHealthChanges -= OnHealthChangesHandler;
            _showable = null;

            DestroyAsPoolableObject();
        }

        /// <summary>
        /// Updates the healthbar position.
        /// </summary>
        private void UpdatePosition(bool smooth = true)
        {
            var targetWorldPosition = _showable?.HealthBarPosition;

            if (targetWorldPosition == null || _parentRectTransform == null)
            {
                this.enabled = false;
                Debug.LogError($"Missing reference at {name}.");
                return;
            }

            var targetScreenPosition = _cameraMain.WorldToViewportPoint(targetWorldPosition.Value);
            var healthBarScreenPosition = new Vector2(
                (targetScreenPosition.x * _parentRectTransform.sizeDelta.x) - (_parentRectTransform.sizeDelta.x / 2f),
                (targetScreenPosition.y * _parentRectTransform.sizeDelta.y) - (_parentRectTransform.sizeDelta.y / 2f) + _heightOffset);

            // Using Vector2.Lerp here to avoid health bar jerking.
            _rectTransform.anchoredPosition = smooth ?
                Vector2.Lerp(_rectTransform.anchoredPosition, healthBarScreenPosition, POSITION_LERP_SPEED * Time.deltaTime)
                : healthBarScreenPosition;
        }

        /// <summary>
        /// Subscribes for health changes.
        /// </summary>
        /// <exception cref="NullReferenceException">showable</exception>
        private void SubscribeForHealthChanges()
        {
            if (_showable == null)
                throw new NullReferenceException(nameof(_showable));

            _showable.OnHealthChanges += OnHealthChangesHandler;
            _showable.OnHealthStopShowing += OnHealthStopShowingHandler;
        }

        /// <summary>
        /// Unsubscribes for health changes.
        /// </summary>
        private void UnsubscribeForHealthChanges()
        {
            if (_showable == null)
                return;

            _showable.OnHealthChanges -= OnHealthChangesHandler;
            _showable.OnHealthStopShowing -= OnHealthStopShowingHandler;
        }

        /// <summary>
        /// Sets the new size delta.
        /// </summary>
        /// <param name="xSize">Size of the x.</param>
        /// <param name="ySize">Size of the y.</param>
        private void SetNewSizeDelta(float xSize, float ySize)
        {
            _healthImage.rectTransform.sizeDelta = _rectTransform.sizeDelta = new Vector2(xSize, ySize);
        }
    }
}
