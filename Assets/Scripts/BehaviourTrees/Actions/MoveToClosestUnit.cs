using System.Linq;
using UnityEngine;

namespace Assets.Scripts.BehaviourTrees.Actions
{
    /// <summary>
    /// Moving node: the unit selects closest unit of a given team and move to it.
    /// </summary>
    /// <seealso cref="MoveTo" />
    internal class MoveToClosestUnit : MoveTo
    {
        private Team _unitTeam;

        /// <summary>
        /// Initializes a new instance of the <see cref="MoveToClosestUnit"/> class.
        /// </summary>
        /// <param name="unitTeam">The closest unit's team.</param>
        public MoveToClosestUnit(Team unitTeam) => _unitTeam = unitTeam;

        /// <summary>
        /// Gets the closest unit-target of a given team.
        /// </summary>
        /// <param name="unit">The controllable unit.</param>
        /// <param name="context">The environment context.</param>
        /// <returns>The target.</returns>
        protected override ITarget GetTarget(Unit unit, IBehaviourContext context)
        {
            var units = context.UnitsCollector.GetUnitsByTeam(_unitTeam);
            var closest = units
                .OrderBy(x => Vector3.Distance(x.Position, unit.Position))
                .FirstOrDefault();

            return closest;
        }
    }
}
