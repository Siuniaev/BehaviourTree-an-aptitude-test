using Assets.Scripts.BehaviourTrees;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// The skill data of Healing Skill.
    /// </summary>
    /// <seealso cref="SkillData" />
    [CreateAssetMenu(menuName = "Skills/Heal", fileName = "New Heal Skill")]
    internal class SkillHeal : SkillData
    {
        public const int HEALING_VALUE_MIN = 1;

        [SerializeField] private int _healingValue;

        /// <summary>
        /// Checks the correctness of the entered data.
        /// </summary>
        private void OnValidate()
        {
            _healingValue = Mathf.Max(_healingValue, HEALING_VALUE_MIN);
        }

        /// <summary>
        /// Uses the skill on the given target.
        /// </summary>
        /// <param name="target">The target.</param>
        public override void UseSkillOn(ITarget target)
        {
            if (target is Unit targetUnit)
                targetUnit.ApplyHeal(_healingValue);
        }

        /// <summary>
        /// Gets the suitable target for this skill.
        /// </summary>
        /// <param name="unit">The controllable unit.</param>
        /// <param name="context">The environment context.</param>
        /// <returns>The target.</returns>
        public override Unit GetTargetForSkill(Unit unit, IBehaviourContext context)
        {
            var target = context.UnitsCollector.GetUnitsByTeam(unit.Team)
                .Where(x => x.IsAlmostDead)
                .OrderBy(x => x.HealPoints)
                .FirstOrDefault();

            return target;
        }
    }
}
