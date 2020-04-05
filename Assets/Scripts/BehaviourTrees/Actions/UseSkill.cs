using UnityEngine;

namespace Assets.Scripts.BehaviourTrees.Actions
{
    /// <summary>
    /// The using skill node: unit select closest suitable target and uses skill on it.
    /// </summary>
    /// <seealso cref="Action" />
    internal class UseSkill : Action
    {
        private bool _isTargetSelected;

        /// <summary>
        /// Runs this node.
        /// </summary>
        /// <param name="unit">The controllable unit.</param>
        /// <param name="context">The environment context.</param>
        /// <returns>The node execution result.</returns>
        public override ActionState Run(Unit unit, IBehaviourContext context)
        {
            // Select target.
            if (!_isTargetSelected)
            {
                var target = unit.SkillData?.GetTargetForSkill(unit, context);

                if (target == null
                    || Vector3.Distance(target.Position, unit.Position) > unit.Parameters.AttackRange)
                    return ActionState.Failure;

                unit.SetTarget(target);
                _isTargetSelected = true;
            }

            // Check if can use skill on target.
            if (!unit.CanAttackTarget())
            {
                _isTargetSelected = false;
                return ActionState.Failure;
            }

            // Rotate to target.
            if (!unit.LookAtTarget())
                return ActionState.Running;

            // Use skill on target.
            unit.UseSkillOnTarget();
            _isTargetSelected = false;
            return ActionState.Success;
        }
    }
}
