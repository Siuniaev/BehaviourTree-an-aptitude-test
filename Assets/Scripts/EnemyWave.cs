using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// The data of one wave of enemies.
    /// </summary>
    [Serializable]
    internal class EnemyWave
    {
        [SerializeField] private UnitData[] _unitData;

        /// <summary>
        /// Gets the units' data.
        /// </summary>
        /// <value>The units' data.</value>
        public IEnumerable<UnitData> UnitData => _unitData;
    }
}
