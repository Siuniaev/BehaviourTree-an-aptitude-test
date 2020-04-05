namespace Assets.Scripts
{
    /// <summary>
    /// Begins to spawn enemies in waves and stops spawn.
    /// </summary>
    /// <seealso cref="IUnitsSpawner" />
    internal interface IEnemiesSpawner : IUnitsSpawner
    {
        /// <summary>
        /// Starts the spawning enemies.
        /// </summary>
        /// <param name="waveIndex">Index of the wave.</param>
        void StartSpawningEnemies(int waveIndex);

        /// <summary>
        /// Stops the spawning enemies.
        /// </summary>
        void StopSpawningEnemies();
    }
}