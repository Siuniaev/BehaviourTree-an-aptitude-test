namespace Assets.Scripts.BehaviourTrees.Conditions
{
    /// <summary>
    /// Checks if there is a suitable unit for applying the skill by a controllable unit.
    /// </summary>
    /// <seealso cref="Condition" />
    internal class IsSkillTargetExist : Condition
    {
        /// <summary>
        /// Checks if there is a suitable unit for applying the skill by a controllable unit.
        /// </summary>
        /// <param name="unit">The controllable unit.</param>
        /// <param name="context">The environment context.</param>
        /// <returns>
        ///   <c>true</c>if the condition is satisfied; otherwise, <c>false</c>.
        /// </returns>
        protected override bool Check(Unit unit, IBehaviourContext context)
        {
            var target = unit.SkillData?.GetTargetForSkill(unit, context);

            return target != null;
        }
    }
}
