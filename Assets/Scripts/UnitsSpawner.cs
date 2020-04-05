using Assets.Scripts.BehaviourTrees;
using Assets.Scripts.DI;
using Assets.Scripts.UI;
using System;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// The base class for unit's spawners. Create units using ObjectsPool and customize them.
    /// </summary>    
    /// <seealso cref="IUnitsSpawner" />
    internal abstract class UnitsSpawner : MonoBehaviour, IUnitsSpawner
    {
        [Injection] protected IObjectsPool ObjectsPool { get; set; }
        [Injection] protected IHUD HUD { get; set; }
        [Injection] protected ILevelMarkersProvider LevelMarkers { get; set; }
        [Injection] protected IUnitsCollector UnitsCollector { get; set; }

        [SerializeField] protected BehaviourTreeData _behaviourData;

        /// <summary>
        /// Occurs when the unit has spawned.
        /// </summary>
        public event Action<Unit> OnUnitSpawned;

        /// <summary>
        /// Gets the units team.
        /// </summary>
        /// <value>The units team.</value>
        public abstract Team UnitsTeam { get; }

        /// <summary>
        /// Is called on the frame when a script is enabled just before any of the Update methods are called the first time.
        /// </summary>
        /// <exception cref="System.NullReferenceException">_behaviourData</exception>
        private void Start() => CheckReferences();

        /// <summary>
        /// Checks the missing references.
        /// </summary>
        /// <exception cref="System.NullReferenceException">_behaviourData</exception>
        protected virtual void CheckReferences()
        {
            if (_behaviourData == null)
                throw new NullReferenceException(nameof(_behaviourData));
        }

        /// <summary>
        /// Spawns the one unit.
        /// </summary>
        /// <param name="data">The unit data.</param>
        /// <param name="position">The spawned position.</param>
        /// <returns>The spawned unit.</returns>
        protected Unit SpawnOneUnit(UnitData data, Vector3 position)
        {
            var unit = ObjectsPool.GetOrCreate<Unit>(data.Prefab, position, Quaternion.identity);

            if (!unit.IsSameUnitDataAlreadySet(data))
                unit.SetUnitData(data);

            unit.SetTeam(UnitsTeam);
            var context = new BehaviourContext(LevelMarkers, UnitsCollector);
            unit.SetBehaviour(_behaviourData.CreateBehaviourTree(context));
            HUD.CreateHealthBarFor(unit);
            unit.OnSpawnedUpdate();
            OnUnitSpawned?.Invoke(unit);

            return unit;
        }
    }
}
