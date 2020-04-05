namespace Assets.Scripts.BehaviourTrees.Actions
{
    /// <summary>
    /// The base node for all Move-actions in BehaviourTree: unit select target and move to target.
    /// </summary>
    /// <seealso cref="Action" />
    internal abstract class MoveTo : Action
    {
        /// <summary>
        /// Runs this node.
        /// </summary>
        /// <param name="unit">The controllable unit.</param>
        /// <param name="context">The environment context.</param>
        /// <returns>The node execution result.</returns>
        public override ActionState Run(Unit unit, IBehaviourContext context)
        {
            var target = GetTarget(unit, context);

            if (target == null)
                return ActionState.Failure;

            unit.SetTarget(target);
            unit.MoveToTarget();

            return ActionState.Success;
        }

        /// <summary>
        /// Selecting target function.
        /// </summary>
        /// <param name="unit">The controllable unit.</param>
        /// <param name="context">The environment context.</param>
        /// <returns>The selected target.</returns>
        protected abstract ITarget GetTarget(Unit unit, IBehaviourContext context);
    }
}
