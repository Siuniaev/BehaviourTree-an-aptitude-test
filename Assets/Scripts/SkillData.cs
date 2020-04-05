using Assets.Scripts.BehaviourTrees;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// The base class for skill datas.
    /// </summary>        
    internal abstract class SkillData : ScriptableObject
    {
        /// <summary>
        /// Uses the skill on the given target.
        /// </summary>
        /// <param name="target">The target.</param>
        public abstract void UseSkillOn(ITarget target);

        /// <summary>
        /// Gets the suitable target for this skill.
        /// </summary>
        /// <param name="unit">The controllable unit.</param>
        /// <param name="context">The environment context.</param>
        /// <returns>The target.</returns>
        public abstract Unit GetTargetForSkill(Unit unit, IBehaviourContext context);
    }
}
