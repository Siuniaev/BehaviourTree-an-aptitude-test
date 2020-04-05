using System;

namespace Assets.Scripts.BehaviourTrees
{
    /// <summary>
    /// The environment context of BehaviourTree, providing relevant game scene data for decisions making.
    /// </summary>
    /// <seealso cref="IBehaviourContext" />
    internal class BehaviourContext : IBehaviourContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BehaviourContext"/> class.
        /// </summary>
        /// <param name="levelMarkers">The level markers provider.</param>
        /// <param name="unitsCollector">The units collector.</param>
        /// <exception cref="System.NullReferenceException">levelMarkers</exception>
        /// <exception cref="System.ArgumentNullException">unitsCollector</exception>
        public BehaviourContext(ILevelMarkersProvider levelMarkers, IUnitsCollector unitsCollector)
        {
            LevelMarkers = levelMarkers ?? throw new NullReferenceException(nameof(levelMarkers));
            UnitsCollector = unitsCollector ?? throw new ArgumentNullException(nameof(unitsCollector));
        }

        /// <summary>
        /// Gets the level markers provider: finish-area, spawn positions, etc.
        /// <seealso cref="ILevelMarkersProvider" />
        /// </summary>
        /// <value>The level markers provider.</value>
        public ILevelMarkersProvider LevelMarkers { get; private set; }

        /// <summary>
        /// Gets the units collector: gives access to all units in the scene.
        /// <seealso cref="IUnitsCollector" />
        /// </summary>
        /// <value>The units collector.</value>
        public IUnitsCollector UnitsCollector { get; private set; }
    }
}
