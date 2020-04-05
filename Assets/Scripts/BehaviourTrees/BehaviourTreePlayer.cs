using Assets.Scripts.BehaviourTrees.Actions;
using Assets.Scripts.BehaviourTrees.Composites;
using Assets.Scripts.BehaviourTrees.Conditions;
using UnityEngine;

namespace Assets.Scripts.BehaviourTrees
{
    /// <summary>
    /// The behavior tree data for player's units.
    /// </summary>
    /// <seealso cref="BehaviourTreeData" />
    [CreateAssetMenu(menuName = "AI/BehaviourTreePlayer", fileName = "new BehaviourTreePlayer")]
    internal class BehaviourTreePlayer : BehaviourTreeData
    {
        /// <summary>
        /// Creates the behaviour tree for player's units.
        /// </summary>
        /// <param name="context">The environment context.</param>
        /// <returns>The behavioral tree.</returns>
        public override BehaviourTree CreateBehaviourTree(IBehaviourContext context)
        {
            var rootNode = new RootNode(
                new Selector(
                    new Sequence(
                        new IsReadyToUsePower(),
                        new Selector(
                            new Sequence(
                                new IsCloseToSkillTarget(),
                                new UseSkill()
                                ),
                            new Sequence(
                                new IsSkillTargetExist(),
                                new MoveToSkillTarget()
                                )
                            )
                        ),
                    new Sequence(
                        new IsReadyToAttack(),
                        new Selector(
                            new Sequence(
                                new IsCloseToAttackUnit(Team.Enemies),
                                new Attack(Team.Enemies)
                                ),
                            new Sequence(
                                new IsCloseToGetScared(Team.Enemies),
                                new MoveToClosestUnit(Team.Enemies)
                                )
                            )
                        ),
                    new Sequence(
                        new IsCloseToGetScared(Team.Enemies),
                        new MoveToSafety(Team.Enemies, 0.5f)
                        ),
                    new MoveToFinishArea()
                    )
                );

            return new BehaviourTree(context, rootNode);
        }
    }
}
