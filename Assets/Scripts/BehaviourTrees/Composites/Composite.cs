using System.Collections.Generic;

namespace Assets.Scripts.BehaviourTrees.Composites
{
    /// <summary>
    /// The base node for all composite nodes in BehaviourTree.
    /// A node that defines the root of a an individual branch and the basic rules for executing this branch.
    /// </summary>
    /// <seealso cref="Node" />
    internal abstract class Composite : Node
    {
        protected List<Node> _childrens;

        /// <summary>
        /// Initializes a new instance of the <see cref="Composite"/> class.
        /// </summary>
        /// <param name="nodes">The children nodes.</param>
        protected Composite(params Node[] nodes)
        {
            _childrens = new List<Node>(nodes);
        }

        /// <summary>
        /// Gets or sets the current running node in this composite node.
        /// Once running the child-node continues to run until it returns a failure or success.
        /// </summary>
        /// <value>The current running node.</value>
        public Node RunningNode { get; protected set; }

        /// <summary>
        /// Runs this node.
        /// </summary>
        /// <param name="unit">The controllable unit.</param>
        /// <param name="context">The environment context.</param>
        /// <returns>The node execution result.</returns>
        public sealed override ActionState Run(Unit unit, IBehaviourContext context)
        {
            // The priority is the execution of an already running node.
            if (RunningNode != null)
            {
                var result = RunningNode.Run(unit, context);

                if (result != ActionState.Running)
                    RunningNode = null;

                return result;
            }

            return RunChildrenNodes(unit, context);
        }

        /// <summary>
        /// Runs the children nodes.
        /// </summary>
        /// <param name="unit">The controllable unit.</param>
        /// <param name="context">The environment context.</param>
        /// <returns>The node execution result.</returns>
        protected abstract ActionState RunChildrenNodes(Unit unit, IBehaviourContext context);
    }
}
