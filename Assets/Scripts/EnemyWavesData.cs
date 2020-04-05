using System;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// The data of all waves of enemies in the game.
    /// </summary>    
    [CreateAssetMenu(menuName = "EnemyWaves", fileName = "New EnemyWaves Data")]
    internal class EnemyWavesData : ScriptableObject
    {
        [SerializeField] private EnemyWave[] _enemyWaves;

        /// <summary>
        /// Checks the correctness of the entered data.
        /// </summary>
        private void OnValidate()
        {
            if (_enemyWaves.Length == 0)
                Debug.LogError($"{nameof(_enemyWaves)} is empty!");
        }

        /// <summary>
        /// Gets the one wave of enemies' data.
        /// </summary>
        /// <param name="index">The wave index.</param>
        /// <returns>The data of one wave of enemies (or null).</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">index</exception>
        public EnemyWave GetWave(int index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (_enemyWaves.Length == 0)
                return null;

            if (index >= _enemyWaves.Length - 1)
                return _enemyWaves[_enemyWaves.Length - 1];

            return _enemyWaves[index];
        }
    }
}
