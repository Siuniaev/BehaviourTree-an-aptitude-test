using System;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// The area crossing which the player wins the round.
    /// </summary>
    /// <seealso cref="Area" />
    internal class FinishArea : Area
    {
        [SerializeField] private Team _finishTeam;

        /// <summary>
        /// Occurs when area is crossing by player's unit.
        /// </summary>
        public event Action<Unit> OnFinished;

        /// <summary>
        /// Is called when this collider/rigidbody has begun touching another rigidbody/collider.
        /// </summary>
        /// <param name="other">The other collider.</param>
        private void OnTriggerEnter(Collider other)
        {
            var unit = other.GetComponent<Unit>();

            if (unit == null || unit.Team != _finishTeam)
                return;

            OnFinished?.Invoke(unit);
        }

        /// <summary>
        /// Gets this finish area as a target.
        /// </summary>
        /// <returns>The point-target.</returns>
        public ITarget GetTarget() => new PointTarget(_collider.bounds.center);
    }
}
