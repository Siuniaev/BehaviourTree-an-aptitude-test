using Assets.Scripts.BehaviourTrees.Composites;
using System;

namespace Assets.Scripts.BehaviourTrees
{
    /// <summary>
    /// The root node of BehaviourTree, is the initial node of behavior selecting.
    /// </summary>
    /// <seealso cref="Node" />
    internal class RootNode : Node
    {
        private Composite _child;

        /// <summary>
        /// Initializes a new instance of the <see cref="RootNode"/> class.
        /// </summary>
        /// <param name="childCompositeNode">The child <see cref="Composite" /> node.</param>
        /// <exception cref="System.ArgumentNullException">childCompositeNode</exception>
        public RootNode(Composite childCompositeNode)
        {
            _child = childCompositeNode ?? throw new ArgumentNullException(nameof(childCompositeNode));
        }

        /// <summary>
        /// Runs this child node.
        /// </summary>
        /// <param name="unit">The controllable unit.</param>
        /// <param name="context">The environment context.</param>
        /// <returns>The node execution result.</returns>
        public override ActionState Run(Unit unit, IBehaviourContext context)
        {
            return _child.Run(unit, context);
        }
    }
}
