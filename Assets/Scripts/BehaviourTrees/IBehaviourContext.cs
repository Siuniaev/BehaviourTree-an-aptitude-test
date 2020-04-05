namespace Assets.Scripts.BehaviourTrees
{
    /// <summary>
    /// The environment context of BehaviourTree, providing relevant game scene data for decisions making.
    /// </summary>
    internal interface IBehaviourContext
    {
        /// <summary>
        /// Gets the level markers provider: finish-area, spawn positions, etc.
        /// <seealso cref="ILevelMarkersProvider" />
        /// </summary>
        /// <value>The level markers provider.</value>
        ILevelMarkersProvider LevelMarkers { get; }

        /// <summary>
        /// Gets the units collector: gives access to all units in the scene.
        /// <seealso cref="IUnitsCollector" />
        /// </summary>
        /// <value>The units collector.</value>
        IUnitsCollector UnitsCollector { get; }
    }
}
