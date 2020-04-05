using UnityEngine;

namespace Assets.Scripts.BehaviourTrees.Conditions
{
    /// <summary>
    /// Checks if the unit of a given team is close for be skill-casted by controllable unit.
    /// </summary>
    /// <seealso cref="Condition" />
    internal class IsCloseToSkillTarget : Condition
    {
        /// <summary>
        /// Checks if the unit of a given team is close for be skill-casted by controllable unit.
        /// </summary>
        /// <param name="unit">The controllable unit.</param>
        /// <param name="context">The environment context.</param>
        /// <returns>
        ///   <c>true</c>if the condition is satisfied; otherwise, <c>false</c>.
        /// </returns>
        protected override bool Check(Unit unit, IBehaviourContext context)
        {
            var target = unit.SkillData?.GetTargetForSkill(unit, context);

            return target != null
                && Vector3.Distance(target.Position, unit.Position) <= unit.Parameters.AttackRange;
        }
    }
}
