namespace Assets.Scripts.BehaviourTrees.Composites
{
    /// <summary>
    /// Executes child nodes from left to right and stops their execution when one of them is not running.
    /// </summary>
    /// <seealso cref="Composite" />
    internal class Sequence : Composite
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sequence"/> class.
        /// </summary>
        /// <param name="nodes">The children nodes.</param>
        public Sequence(params Node[] nodes) : base(nodes) { }

        /// <summary>
        /// Runs the children nodes.
        /// </summary>
        /// <param name="unit">The controllable unit.</param>
        /// <param name="context">The environment context.</param>
        /// <returns>The node execution result.</returns>
        protected override ActionState RunChildrenNodes(Unit unit, IBehaviourContext context)
        {
            for (int i = 0; i < _childrens.Count; i++)
            {
                var result = _childrens[i].Run(unit, context);

                switch (result)
                {
                    case ActionState.Running:
                        RunningNode = _childrens[i];
                        return result;

                    case ActionState.Success:
                        RunningNode = null;
                        continue;

                    case ActionState.Failure:
                        RunningNode = null;
                        return result;
                }
            }

            return ActionState.Success;
        }
    }
}
