using Assets.Scripts.BehaviourTrees.Actions;
using Assets.Scripts.BehaviourTrees.Composites;
using Assets.Scripts.BehaviourTrees.Conditions;
using UnityEngine;

namespace Assets.Scripts.BehaviourTrees
{
    /// <summary>
    /// The behavior tree data for enemy units.
    /// </summary>
    /// <seealso cref="BehaviourTreeData" />
    [CreateAssetMenu(menuName = "AI/BehaviourTreeEnemy", fileName = "new BehaviourTreeEnemy")]
    internal class BehaviourTreeEnemy : BehaviourTreeData
    {
        /// <summary>
        /// Creates the behaviour tree for enemy units.
        /// </summary>
        /// <param name="context">The environment context.</param>
        /// <returns>The behavioral tree.</returns>
        public override BehaviourTree CreateBehaviourTree(IBehaviourContext context)
        {
            var rootNode = new RootNode(
                new Selector(
                    new Sequence(
                        new IsCloseToAttackUnit(Team.Player),
                        new Attack(Team.Player)
                        ),
                    new MoveToClosestUnit(Team.Player),
                    new DoNothing()
                    )
                );

            return new BehaviourTree(context, rootNode);
        }
    }
}
