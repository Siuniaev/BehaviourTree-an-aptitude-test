namespace Assets.Scripts
{
    /// <summary>
    /// Spawns player's units.
    /// </summary>
    /// <seealso cref="IUnitsSpawner" />
    internal interface IPlayerUnitsSpawner : IUnitsSpawner
    {
        /// <summary>
        /// Spawns the player's units.
        /// </summary>
        void SpawnUnits();
    }
}
