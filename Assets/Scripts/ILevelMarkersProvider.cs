using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Povides access to the level markers/spots.
    /// </summary>
    internal interface ILevelMarkersProvider
    {
        /// <summary>
        /// Gets the finish area.
        /// </summary>
        /// <value>The finish area.</value>
        FinishArea FinishArea { get; }

        /// <summary>
        /// Gets the next player unit spawn position.
        /// </summary>
        /// <returns>The spawn point.</returns>
        Vector3 GetPlayerUnitSpawnPosition();

        /// <summary>
        /// Gets the next enemy spawn position.
        /// </summary>
        /// <returns>The spawn point.</returns>
        Vector3 GetEnemySpawnPosition();
    }
}
