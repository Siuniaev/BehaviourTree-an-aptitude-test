using Assets.Scripts.Extensions;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Povides access to the level markers/spots.
    /// </summary>    
    /// <seealso cref="ILevelMarkersProvider" />
    internal sealed class LevelMarkersProvider : MonoBehaviour, ILevelMarkersProvider
    {
        [SerializeField] private FinishArea _finishArea;
        [SerializeField] private PlayerUnitsSpawnArea _startArea;
        [SerializeField] private EnemySpawnPoint[] _enemiesSpawnPoints;
        private int _nextEnemySpawnPointIndex;

        /// <summary>
        /// Gets the finish area.
        /// </summary>
        /// <value>The finish area.</value>
        public FinishArea FinishArea => _finishArea;

        /// <summary>
        /// Is called on the frame when a script is enabled just before any of the Update methods are called the first time.
        /// </summary>
        private void Start()
        {
            CheckReferences();
            _enemiesSpawnPoints.Shuffle();
        }

        /// <summary>
        /// Gets the next player unit spawn position.
        /// </summary>
        /// <returns>The spawn point.</returns>
        public Vector3 GetPlayerUnitSpawnPosition()
        {
            return _startArea.GetRandomSpawnPosition();
        }

        /// <summary>
        /// Gets the next enemy spawn position.
        /// </summary>
        /// <returns>The spawn point.</returns>
        public Vector3 GetEnemySpawnPosition()
        {
            var point = _enemiesSpawnPoints[_nextEnemySpawnPointIndex];

            _nextEnemySpawnPointIndex++;
            if (_nextEnemySpawnPointIndex >= _enemiesSpawnPoints.Length)
            {
                _nextEnemySpawnPointIndex = 0;
                _enemiesSpawnPoints.Shuffle();
            }

            return point.transform.position;
        }

        /// <summary>
        /// Checks the specified references.
        /// </summary>
        private void CheckReferences()
        {
            LeaveTheOnlyObject(ref _finishArea);
            LeaveTheOnlyObject(ref _startArea);
            CheckEnemiesSpawnPoints();
        }

        /// <summary>
        /// Leaves the only one object in the scene of the given type.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <param name="reference">The reference.</param>
        /// <exception cref="UnityException">Can't find {typeof(T).Name} instances in scene!</exception>
        private void LeaveTheOnlyObject<T>(ref T reference)
            where T : MonoBehaviour
        {
            var finded = FindObjectsOfType<T>();

            if (reference == null)
                reference = finded?.FirstOrDefault() ?? throw new UnityException($"Can't find {typeof(T).Name} instances in scene!");

            var copyRef = reference;

            if (finded != null && finded.Length > 1)
                finded.Where(x => x != copyRef).ForEach(x => x.gameObject.SetActive(false));
        }

        /// <summary>
        /// Checks the enemies spawn points.
        /// </summary>
        /// <exception cref="UnityException">Can't find {typeof(EnemySpawnPoint).Name} instances in scene!</exception>
        private void CheckEnemiesSpawnPoints()
        {
            if (_enemiesSpawnPoints.Length != 0)
                return;

            _enemiesSpawnPoints = FindObjectsOfType<EnemySpawnPoint>();

            if (_enemiesSpawnPoints.Length == 0)
                throw new UnityException($"Can't find {typeof(EnemySpawnPoint).Name} instances in scene!");
        }
    }
}
