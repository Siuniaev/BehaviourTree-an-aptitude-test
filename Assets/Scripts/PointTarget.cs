using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// The point in space that can be used as a target.
    /// </summary>
    /// <seealso cref="ITarget" />
    internal class PointTarget : ITarget
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PointTarget"/> class.
        /// </summary>
        /// <param name="point">The point coordinates.</param>
        public PointTarget(Vector3 point) => Position = point;

        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <value>The position.</value>
        public Vector3 Position { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is alive.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is alive; otherwise, <c>false</c>.
        /// </value>
        public bool IsAlive => true;

        /// <summary>
        /// Applies the given damage to this target.
        /// </summary>
        /// <param name="damage">The damage.</param>
        public void ApplyDamage(float damage) { }
    }
}
