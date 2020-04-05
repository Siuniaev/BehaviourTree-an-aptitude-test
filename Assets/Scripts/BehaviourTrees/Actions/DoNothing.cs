namespace Assets.Scripts.BehaviourTrees.Actions
{
    /// <summary>
    /// The do nothing node, unit resets his target and stay.
    /// </summary>
    /// <seealso cref="Action" />
    internal class DoNothing : Action
    {
        public override ActionState Run(Unit unit, IBehaviourContext context)
        {
            unit.ResetTarget();
            return ActionState.Success;
        }
    }
}
