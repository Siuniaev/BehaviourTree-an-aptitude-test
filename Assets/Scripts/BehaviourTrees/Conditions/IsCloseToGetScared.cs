using System.Linq;
using UnityEngine;

namespace Assets.Scripts.BehaviourTrees.Conditions
{
    /// <summary>
    /// Checks if the unit of a given team is close for attack the controllable unit.
    /// </summary>
    /// <seealso cref="Condition" />
    internal class IsCloseToGetScared : Condition
    {
        private Team _teamToScare;

        /// <summary>
        /// Initializes a new instance of the <see cref="IsCloseToGetScared"/> class.
        /// </summary>
        /// <param name="teamToScare">The team to scare.</param>
        public IsCloseToGetScared(Team teamToScare) => _teamToScare = teamToScare;

        /// <summary>
        /// Checks if the unit of a given team is close for attack the controllable unit.
        /// </summary>
        /// <param name="unit">The controllable unit.</param>
        /// <param name="context">The environment context.</param>
        /// <returns>
        ///   <c>true</c>if the condition is satisfied; otherwise, <c>false</c>.
        /// </returns>
        protected override bool Check(Unit unit, IBehaviourContext context)
        {
            var enemies = context.UnitsCollector.GetUnitsByTeam(_teamToScare);
            var dangerEnemies = enemies
                .Where(x => Vector3.Distance(x.Position, unit.Position) <= x.Parameters.AttackRange)
                .FirstOrDefault();

            return dangerEnemies != null;
        }
    }
}
