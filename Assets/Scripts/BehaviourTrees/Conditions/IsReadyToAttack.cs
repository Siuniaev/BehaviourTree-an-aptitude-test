namespace Assets.Scripts.BehaviourTrees.Conditions
{
    /// <summary>
    /// Checks if the controllable unit is ready to attack.
    /// </summary>
    /// <seealso cref="Condition" />
    internal class IsReadyToAttack : Condition
    {
        /// <summary>
        /// Checks if the controllable unit is ready to attack.
        /// </summary>
        /// <param name="unit">The controllable unit.</param>
        /// <param name="context">The environment context.</param>
        /// <returns>
        ///   <c>true</c>if the condition is satisfied; otherwise, <c>false</c>.
        /// </returns>
        protected override bool Check(Unit unit, IBehaviourContext context)
        {
            return unit.IsReadyToAttack;
        }
    }
}
