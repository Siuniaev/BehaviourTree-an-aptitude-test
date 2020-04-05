using System.Linq;
using UnityEngine;

namespace Assets.Scripts.BehaviourTrees.Conditions
{
    /// <summary>
    /// Checks if the unit of a given team is close for be attacked by controllable unit.
    /// </summary>
    /// <seealso cref="Condition" />
    internal class IsCloseToAttackUnit : Condition
    {
        private Team _unitTeam;

        /// <summary>
        /// Initializes a new instance of the <see cref="IsCloseToAttackUnit"/> class.
        /// </summary>
        /// <param name="unitTeam">The attacked unit's team.</param>
        public IsCloseToAttackUnit(Team unitTeam) => _unitTeam = unitTeam;

        /// <summary>
        /// Checks if the unit of a given team is close for be attacked by controllable unit.
        /// </summary>
        /// <param name="unit">The controllable unit.</param>
        /// <param name="context">The environment context.</param>
        /// <returns>
        ///   <c>true</c>if the condition is satisfied; otherwise, <c>false</c>.
        /// </returns>
        protected override bool Check(Unit unit, IBehaviourContext context)
        {
            var units = context.UnitsCollector.GetUnitsByTeam(_unitTeam);
            var closest = units
                .Where(x => Vector3.Distance(x.Position, unit.Position) <= unit.Parameters.AttackRange)
                .FirstOrDefault();

            return closest != null;
        }
    }
}
