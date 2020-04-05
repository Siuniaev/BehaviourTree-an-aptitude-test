using System.Linq;
using UnityEngine;

namespace Assets.Scripts.BehaviourTrees.Actions
{
    /// <summary>
    /// The attacking node: unit select closest target and attack.
    /// </summary>
    /// <seealso cref="Action" />
    internal class Attack : Action
    {
        private Team _teamToAttack;
        private bool _isTargetSelected;

        /// <summary>
        /// Initializes a new instance of the <see cref="Attack"/> class.
        /// </summary>
        /// <param name="teamToAttack">The team to attack.</param>
        public Attack(Team teamToAttack) => _teamToAttack = teamToAttack;

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
                var units = context.UnitsCollector.GetUnitsByTeam(_teamToAttack);
                var closestTarget = units
                    .Where(x => Vector3.Distance(x.Position, unit.Position) <= unit.Parameters.AttackRange)
                    .OrderBy(x => Vector3.Distance(x.Position, unit.Position))
                    .ThenBy(x => x.HealPoints)
                    .FirstOrDefault();

                if (closestTarget == null)
                    return ActionState.Failure;

                unit.SetTarget(closestTarget);
                _isTargetSelected = true;
            }

            // Check if can attack target.
            if (!unit.CanAttackTarget())
            {
                _isTargetSelected = false;
                return ActionState.Failure;
            }

            // Rotate to target.
            if (!unit.LookAtTarget())
                return ActionState.Running;

            // Attack target.
            unit.AttackTarget();
            _isTargetSelected = false;
            return ActionState.Success;
        }
    }
}
