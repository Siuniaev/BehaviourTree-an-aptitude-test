using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    /// <summary>
    /// Area for spawn player's units.
    /// </summary>
    /// <seealso cref="Area" />
    internal class PlayerUnitsSpawnArea : Area
    {
        /// <summary>
        /// Gets the random spawn position in this area.
        /// </summary>
        /// <returns>The span position.</returns>
        public Vector3 GetRandomSpawnPosition()
        {
            var min = _collider.bounds.min;
            var max = _collider.bounds.max;

            return new Vector3(Random.Range(min.x, max.x), min.y, Random.Range(min.z, max.z));
        }
    }
}
