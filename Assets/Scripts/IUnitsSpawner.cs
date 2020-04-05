using System;

namespace Assets.Scripts
{
    /// <summary>
    /// Spawns units.
    /// </summary>
    internal interface IUnitsSpawner
    {
        /// <summary>
        /// Occurs when the unit has spawned.
        /// </summary>
        event Action<Unit> OnUnitSpawned;
    }
}
