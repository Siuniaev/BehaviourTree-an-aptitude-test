using Assets.Scripts.DI;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// The main UI object in the game. Creates and controls all UI objects.
    /// </summary>
    /// <seealso cref="IHUD" />
    /// <seealso cref="IInitiableOnInjecting" />
    internal sealed class HUD : MonoBehaviour, IHUD, IInitiableOnInjecting
    {
        [Injection] private ICameraMain CameraMain { get; set; }
        [Injection] private IObjectsPool ObjectsPool { get; set; }
        [Injection] private IGameReferee GameReferee { get; set; }

        [Header("UI Settings")]
        [SerializeField] private RectTransform _dynamicUICollectorTransform;
        [SerializeField] private HealthBar _healthBarPrefab;
        [SerializeField] private AnimatedText _endGameTextPrefab;
        [SerializeField] private Button _buttonNextPrefab;
        [SerializeField] private Text _roundText;
        private Button _buttonNext;
        private AnimatedText _endGameMessage;

        /// <summary>
        /// Is called when the script instance is being loaded.
        /// </summary>
        private void Awake() => CheckReferences();

        /// <summary>
        /// Initializes this instance immediately after completion of all dependency injection.
        /// </summary>
        public void OnInjected()
        {
            GameReferee.OnRoundEnd += OnRoundEndHandler;
            GameReferee.OnNextRound += OnNextRoundHandler;
        }

        /// <summary>
        /// Called when the attached Behaviour is destroying.
        /// </summary>
        private void OnDestroy()
        {
            GameReferee.OnRoundEnd -= OnRoundEndHandler;
        }

        /// <summary>
        /// Checks the missing references.
        /// </summary>
        private void CheckReferences()
        {
            if (_dynamicUICollectorTransform == null)
                _dynamicUICollectorTransform = GetComponentInChildren<RectTransform>()
                    ?? gameObject.AddComponent<RectTransform>();

            if (_healthBarPrefab == null)
                Debug.LogWarning($"There is no { nameof(_healthBarPrefab) } in { name }");

            if (_endGameTextPrefab == null)
                Debug.LogWarning($"There is no { nameof(_endGameTextPrefab) } in { name }");

            if (_buttonNextPrefab == null)
                Debug.LogWarning($"There is no { nameof(_buttonNextPrefab) } in { name }");

            if (_roundText == null)
                _roundText = GetComponentInChildren<Text>() ?? gameObject.AddComponent<Text>();
        }

        /// <summary>
        /// Called when next round is running.
        /// </summary>
        private void OnNextRoundHandler()
        {
            ShowButoonNext(show: false);
            _endGameMessage?.DestroyAsPoolableObject();
            _roundText.text = GameReferee.CurrentRound.ToString();
        }

        /// <summary>
        /// Creates the health bar for healthed object.
        /// </summary>
        /// <typeparam name="T">The healthed object type.</typeparam>
        /// <param name="targetObj">The target object.</param>
        public void CreateHealthBarFor<T>(T targetObj)
            where T : IShowableHealth
        {
            if (_healthBarPrefab == null)
                return;

            var bar = ObjectsPool.GetOrCreate(_healthBarPrefab);
            bar.Setup(targetObj, _dynamicUICollectorTransform, CameraMain.Camera);
        }

        /// <summary>
        /// Called when the round has ended.
        /// </summary>
        /// <param name="result">The game result.</param>
        private void OnRoundEndHandler(GameResult result)
        {
            _endGameMessage = ObjectsPool.GetOrCreate(_endGameTextPrefab);
            _endGameMessage.Setup(result.ToString() + "!", _dynamicUICollectorTransform);
            _endGameMessage.PlayAnimation();
            ShowButoonNext(show: true);
        }

        /// <summary>
        /// Shows the butoon next.
        /// </summary>
        /// <param name="show">if set to <c>true</c> [show].</param>
        private void ShowButoonNext(bool show)
        {
            if (_buttonNext == null)
                PrepareButtonNext();

            _buttonNext?.gameObject.SetActive(show);
        }

        /// <summary>
        /// Prepares the button "Next".
        /// </summary>
        private void PrepareButtonNext()
        {
            if (_buttonNextPrefab == null)
                return;

            _buttonNext = Instantiate(_buttonNextPrefab, _dynamicUICollectorTransform);
            _buttonNext.onClick.AddListener(GameReferee.RunNextRound);
        }
    }
}
