using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Begins to spawn enemies in waves and stops spawn.
    /// </summary>
    /// <seealso cref="UnitsSpawner" />
    /// <seealso cref="IEnemiesSpawner" />
    internal class EnemiesSpawner : UnitsSpawner, IEnemiesSpawner
    {
        public const float DEFAULT_SPAWNING_WAVES_DELAY = 30f;
        public const float DEFAULT_START_SPAWNING_DELAY = 5f;
        public const float DEFAULT_DELAY_BETWEEN_ENEMIES_CREATION = 0.01f;

        [SerializeField] private float _spawningWavesDelay = DEFAULT_SPAWNING_WAVES_DELAY;
        [SerializeField] private float _startSpawningDelay = DEFAULT_START_SPAWNING_DELAY;
        [SerializeField] private EnemyWavesData _enemyWaves;
        private Coroutine _startedWaveCoroutine;

        /// <summary>
        /// Gets the units team.
        /// </summary>
        /// <value>The units team.</value>
        public override Team UnitsTeam => Team.Enemies;

        /// <summary>
        /// Starts the spawning enemies.
        /// </summary>
        /// <param name="waveIndex">Index of the enemies wave data.</param>
        public void StartSpawningEnemies(int waveIndex)
        {
            StartCoroutine(
                Waiter(_startSpawningDelay, () => _startedWaveCoroutine = StartCoroutine(SpawnWave(waveIndex)))
                );
        }

        /// <summary>
        /// Stops the spawning enemies.
        /// </summary>
        public void StopSpawningEnemies()
        {
            if (_startedWaveCoroutine != null)
                StopCoroutine(_startedWaveCoroutine);
        }

        /// <summary>
        /// Checks the missing references.
        /// </summary>
        /// <exception cref="System.NullReferenceException">_enemyWaves</exception>
        protected override void CheckReferences()
        {
            base.CheckReferences();

            if (_enemyWaves == null)
                throw new NullReferenceException(nameof(_enemyWaves));
        }

        /// <summary>
        /// Spawns the enemy waves.
        /// </summary>
        /// <returns>The yield instruction.</returns>
        private IEnumerator SpawnWave(int waveIndex)
        {
            var wave = _enemyWaves.GetWave(waveIndex);

            if (wave == null)
                yield break;

            while (true)
            {
                foreach (var data in wave.UnitData)
                {
                    var position = LevelMarkers.GetEnemySpawnPosition();
                    var unit = SpawnOneUnit(data, position);

                    // To reduce lags when a lot of enemies get instantiated.
                    yield return new WaitForSeconds(DEFAULT_DELAY_BETWEEN_ENEMIES_CREATION);
                }

                yield return new WaitForSeconds(_spawningWavesDelay);
            }
        }

        /// <summary>
        /// Invokes the given action after the specified number of seconds.
        /// </summary>
        /// <param name="seconds">The seconds.</param>
        /// <param name="action">The action.</param>        
        /// <returns>The yield instruction.</returns>
        private IEnumerator Waiter(float seconds, Action action)
        {
            yield return new WaitForSeconds(seconds);
            action?.Invoke();
        }
    }
}
