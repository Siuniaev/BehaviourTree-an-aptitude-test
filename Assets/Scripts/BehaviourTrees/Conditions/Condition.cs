namespace Assets.Scripts.BehaviourTrees.Conditions
{
    /// <summary>
    /// The base node for all condition-nodes in BehavioralTree.
    /// </summary>
    /// <seealso cref="Node" />
    internal abstract class Condition : Node
    {
        /// <summary>
        /// Runs this node.
        /// </summary>
        /// <param name="unit">The controllable unit.</param>
        /// <param name="context">The environment context.</param>
        /// <returns>The node execution result.</returns>
        public override ActionState Run(Unit unit, IBehaviourContext context)
        {
            return Check(unit, context) ? ActionState.Success : ActionState.Failure;
        }

        /// <summary>
        /// The condition check function.
        /// </summary>
        /// <param name="unit">The controllable unit.</param>
        /// <param name="context">The environment context.</param>
        /// <returns>
        ///   <c>true</c>if the condition is satisfied; otherwise, <c>false</c>.
        /// </returns>
        protected abstract bool Check(Unit unit, IBehaviourContext context);
    }
}
