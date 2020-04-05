using System.Linq;
using UnityEngine;

namespace Assets.Scripts.BehaviourTrees.Actions
{
    /// <summary>
    /// Moving node: the unit runs away from enemies.
    /// </summary>
    /// <seealso cref="MoveTo" />
    internal class MoveToSafety : MoveTo
    {
        private Team _teamToScare;
        private float _duration;
        private float _lastTargetSelectedTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="MoveToSafety"/> class.
        /// </summary>
        /// <param name="teamToScare">The team to scare.</param>
        /// <param name="duration">The duration of running away.</param>
        public MoveToSafety(Team teamToScare, float duration)
        {
            _teamToScare = teamToScare;
            _duration = duration;
        }

        /// <summary>
        /// Runs this node.
        /// </summary>
        /// <param name="unit">The controllable unit.</param>
        /// <param name="context">The environment context.</param>
        /// <returns>The node execution result.</returns>
        public override ActionState Run(Unit unit, IBehaviourContext context)
        {
            // Select safety target-point.
            if (_lastTargetSelectedTime == 0)
            {
                var target = GetTarget(unit, context);

                if (target == null)
                    return ActionState.Failure;

                unit.SetTarget(target);
                _lastTargetSelectedTime = Time.timeSinceLevelLoad;
            }

            // Moves to safety target-point.
            if (Time.timeSinceLevelLoad - _lastTargetSelectedTime < _duration)
            {
                unit.MoveToTarget();
                return ActionState.Running;
            }
            else
            {
                _lastTargetSelectedTime = 0;
                return ActionState.Success;
            }
        }

        /// <summary>
        /// Gets the calculated safety target-point.
        /// </summary>
        /// <param name="unit">The controllable unit.</param>
        /// <param name="context">The environment context.</param>
        /// <returns>The safety target-point.</returns>
        protected override ITarget GetTarget(Unit unit, IBehaviourContext context)
        {
            var enemies = context.UnitsCollector.GetUnitsByTeam(_teamToScare);
            var dangerEnemies = enemies
                .Where(x => Vector3.Distance(x.Position, unit.Position) <= unit.FearDistance);

            if (!dangerEnemies.Any())
                return null;

            // Find a direction to run away from all the danger enemies at once.
            var myPos = unit.Position;
            Vector3 resultDirection = Vector3.zero;

            foreach (var ene in dangerEnemies)
            {
                var enePos = ene.Position;
                enePos.y = myPos.y; // Height alignment;
                var direction = (myPos - enePos).normalized;
                resultDirection += direction;
            }

            var targetPos = myPos + resultDirection * unit.FearDistance;

            if (!unit.CheckPointForReachability(targetPos))
                return null;

            return new PointTarget(targetPos);
        }
    }
}
