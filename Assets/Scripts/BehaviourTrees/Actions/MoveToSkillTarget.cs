namespace Assets.Scripts.BehaviourTrees.Actions
{
    /// <summary>
    /// Moving node: the unit moves to the target for his skill.
    /// </summary>
    /// <seealso cref="MoveTo" />
    internal class MoveToSkillTarget : MoveTo
    {
        /// <summary>
        /// Gets the suitable target for unit's skill.
        /// </summary>
        /// <param name="unit">The controllable unit.</param>
        /// <param name="context">The environment context.</param>
        /// <returns>The target.</returns>
        protected override ITarget GetTarget(Unit unit, IBehaviourContext context)
        {
            return unit.SkillData?.GetTargetForSkill(unit, context);
        }
    }
}
