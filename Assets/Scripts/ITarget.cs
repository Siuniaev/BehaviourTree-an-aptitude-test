using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Has position coordinates, can be attacked.
    /// </summary>
    internal interface ITarget
    {
        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <value>The position.</value>
        Vector3 Position { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is alive.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is alive; otherwise, <c>false</c>.
        /// </value>
        bool IsAlive { get; }

        /// <summary>
        /// Applies the damage to this target.
        /// </summary>
        /// <param name="damage">The damage.</param>
        void ApplyDamage(float damage);
    }
}
