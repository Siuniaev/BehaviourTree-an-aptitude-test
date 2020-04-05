using Assets.Scripts.DI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// The main camera following the player’s units.
    /// </summary>    
    /// <seealso cref="ICameraMain" />
    /// <seealso cref="IInitiableOnInjecting" />
    [RequireComponent(typeof(Camera))]
    internal class CameraFollowedUnit : MonoBehaviour, ICameraMain, IInitiableOnInjecting
    {
        public const float DEFAULT_SPEED = 1;
        public const float DEFAULT_DISTANCE = 30;
        public const float MIN_SPEED = 0;
        public static readonly Vector3 DEFAULT_CAMERA_POSITION = new Vector3(14f, 38.7f, 19f);
        public static readonly Vector3 DEFAULT_CAMERA_ROTATION = new Vector3(45f, -45f, 0f);

        [Injection] private IPlayerUnitsSpawner PlayerUnitsSpawner { get; set; }

        [SerializeField] private Unit _targetUnit;
        [SerializeField] private float _speed = DEFAULT_SPEED;
        [SerializeField] private float _distance = DEFAULT_DISTANCE;
        [SerializeField] private Vector3 StartedPosition = DEFAULT_CAMERA_POSITION;
        [SerializeField] private Vector3 StartedRotation = DEFAULT_CAMERA_ROTATION;
        private List<Unit> _followedUnits = new List<Unit>();

        /// <summary>
        /// Gets the Camera component.
        /// </summary>
        /// <value>The Camera component.</value>
        public Camera Camera { get; private set; }

        /// <summary>
        /// Checks the correctness of the entered data.
        /// </summary>
        private void OnValidate()
        {
            _speed = Mathf.Max(_speed, MIN_SPEED);
        }

        /// <summary>
        /// Is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            Camera = GetComponent<Camera>();
            Camera.transform.position = StartedPosition;
            Camera.transform.eulerAngles = StartedRotation;
        }

        /// <summary>
        /// Initializes this instance immediately after completion of all dependency injection.
        /// </summary>
        public void OnInjected()
        {
            PlayerUnitsSpawner.OnUnitSpawned += OnUnitSpawnedHandler;
        }

        /// <summary>
        /// Is called after all Update functions have been called.
        /// </summary>
        private void LateUpdate()
        {
            if (_targetUnit != null)
                Follow();
        }

        /// <summary>
        /// Called when the attached Behaviour is destroying.
        /// </summary>
        private void OnDestroy()
        {
            PlayerUnitsSpawner.OnUnitSpawned -= OnUnitSpawnedHandler;
        }

        /// <summary>
        /// Smooth following the selected player's unit.
        /// </summary>
        private void Follow()
        {
            var targetPos = _targetUnit.Position - (transform.rotation * Vector3.forward * _distance);
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * _speed);
        }

        /// <summary>
        /// Called when new player's unit has spawned.
        /// </summary>
        /// <param name="unit">The player's unit.</param>
        private void OnUnitSpawnedHandler(Unit unit)
        {
            unit.OnUnitDie += OnUnitDieHandler;
            _followedUnits.Add(unit);

            if (_targetUnit == null)
                _targetUnit = unit;
        }

        /// <summary>
        /// Called when player's unit has died.
        /// </summary>
        /// <param name="unit">The dead unit.</param>
        private void OnUnitDieHandler(Unit unit)
        {
            unit.OnUnitDie -= OnUnitDieHandler;
            _followedUnits.Remove(unit);

            if (_targetUnit == unit)
                _targetUnit = _followedUnits.FirstOrDefault();
        }
    }
}
