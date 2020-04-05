using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Spawns player's units.
    /// </summary>
    /// <seealso cref="UnitsSpawner" />
    /// <seealso cref="IPlayerUnitsSpawner" />
    internal class PlayerUnitsSpawner : UnitsSpawner, IPlayerUnitsSpawner
    {
        [SerializeField] private UnitData[] _unitData;

        /// <summary>
        /// Gets the units team.
        /// </summary>
        /// <value>The units team.</value>
        public override Team UnitsTeam => Team.Player;

        /// <summary>
        /// Spawns the player's units.
        /// </summary>
        public void SpawnUnits()
        {
            foreach (var data in _unitData)
            {
                var position = LevelMarkers.GetPlayerUnitSpawnPosition();
                var unit = SpawnOneUnit(data, position);
            }
        }

        /// <summary>
        /// Checks the missing references.
        /// </summary>
        /// <exception cref="UnityException"></exception>
        protected override void CheckReferences()
        {
            base.CheckReferences();

            if (_unitData.Length == 0)
                throw new UnityException($"{nameof(_unitData)} is empty!");
        }
    }
}
