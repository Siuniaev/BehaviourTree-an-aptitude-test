using System;

namespace Assets.Scripts
{
    /// <summary>
    /// Controls the achievement of goals by the player. Decides when the round is over, starts the next round.
    /// </summary>
    internal interface IGameReferee
    {
        /// <summary>
        /// Occurs when the round has ended.
        /// </summary>
        event Action<GameResult> OnRoundEnd;

        /// <summary>
        /// Occurs when next round has ranned.
        /// </summary>
        event Action OnNextRound;

        /// <summary>
        /// Gets the current round number.
        /// </summary>
        /// <value>The current round number.</value>
        int CurrentRound { get; }

        /// <summary>
        /// Adds the alive player unit in the scene to to account for the loss.
        /// </summary>
        /// <param name="unit">The alive player's unit.</param>
        void AddAlivePlayerUnit(Unit unit);

        /// <summary>
        /// Deletes a deceased unit from the alived units list and ends the round if there are no survivors.
        /// </summary>
        /// <param name="unit">The alive player's unit.</param>
        void RemoveAlivePlayerUnit(Unit unit);

        /// <summary>
        /// Runs the next round.
        /// </summary>
        void RunNextRound();
    }
}
