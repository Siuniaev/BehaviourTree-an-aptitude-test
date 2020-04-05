namespace Assets.Scripts.BehaviourTrees
{
    /// <summary>
    /// The base class for BehaviourTree nodes.
    /// </summary>
    internal abstract class Node
    {
        /// <summary>
        /// Runs this node.
        /// </summary>
        /// <param name="unit">The controllable unit.</param>
        /// <param name="context">The environment context.</param>
        /// <returns>The node execution result.</returns>
        public virtual ActionState Run(Unit unit, IBehaviourContext context)
        {
            return ActionState.Failure;
        }
    }
}
