namespace Assets.Scripts
{
    /// <summary>
    /// Controls the unit.
    /// </summary>
    internal interface IUnitBehaviour
    {
        /// <summary>
        /// Applies the behaviour to unit.
        /// </summary>
        /// <param name="unit">The controllable unit.</param>
        void ApplyBehaviour(Unit unit);
    }
}
