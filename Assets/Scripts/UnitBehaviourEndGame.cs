namespace Assets.Scripts
{
    /// <summary>
    /// Controls the unit in the end of the round, make him dance.
    /// </summary>
    /// <seealso cref="IUnitBehaviour" />
    internal class UnitBehaviourEndGame : IUnitBehaviour
    {
        /// <summary>
        /// Applies the dance-behaviour to unit.
        /// </summary>
        /// <param name="unit">The controllable unit.</param>
        public void ApplyBehaviour(Unit unit) => unit.Dance();
    }
}
