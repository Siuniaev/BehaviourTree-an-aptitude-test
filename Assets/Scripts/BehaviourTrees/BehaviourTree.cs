using System;

namespace Assets.Scripts.BehaviourTrees
{
    /// <summary>
    /// Controls the unit by switching and running behaviour-nodes.
    /// </summary>
    /// <seealso cref="IUnitBehaviour" />
    internal class BehaviourTree : IUnitBehaviour
    {
        protected IBehaviourContext _context;
        protected RootNode _rootNode;

        /// <summary>
        /// Initializes a new instance of the <see cref="BehaviourTree"/> class.
        /// </summary>
        /// <param name="context">The environment context.</param>
        /// <param name="rootNode">The root node.</param>
        /// <exception cref="System.ArgumentNullException">
        /// context
        /// or
        /// rootNode
        /// </exception>
        public BehaviourTree(IBehaviourContext context, RootNode rootNode)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _rootNode = rootNode ?? throw new ArgumentNullException(nameof(rootNode));
        }

        /// <summary>
        /// Applies the behaviour to unit.
        /// </summary>
        /// <param name="unit">The controllable unit.</param>
        public void ApplyBehaviour(Unit unit)
        {
            _rootNode.Run(unit, _context);
        }
    }
}
