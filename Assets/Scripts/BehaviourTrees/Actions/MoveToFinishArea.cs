namespace Assets.Scripts.BehaviourTrees.Actions
{
    /// <summary>
    /// Moving node: the unit move to the finish-area.
    /// </summary>
    /// <seealso cref="MoveTo" />
    internal class MoveToFinishArea : MoveTo
    {
        /// <summary>
        /// Gets the finish area target.
        /// </summary>
        /// <param name="unit">The controllable unit.</param>
        /// <param name="context">The environment context.</param>
        /// <returns>The finish area target.</returns>
        protected override ITarget GetTarget(Unit unit, IBehaviourContext context)
        {
            return context.LevelMarkers.FinishArea.GetTarget();
        }
    }
}
