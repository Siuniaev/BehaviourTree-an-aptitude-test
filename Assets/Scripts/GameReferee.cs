using Assets.Scripts.DI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Controls the achievement of goals by the player. Decides when the round is over, starts the next round.
    /// </summary>    
    /// <seealso cref="IGameReferee" />
    /// <seealso cref="IInitiableOnInjecting" />
    internal sealed class GameReferee : MonoBehaviour, IGameReferee, IInitiableOnInjecting
    {
        [Injection] private ILevelMarkersProvider LevelMarkers { get; set; }

        private List<Unit> _aliveUnits = new List<Unit>();
        private GameResult? _currentGameResult;

        /// <summary>
        /// Occurs when the round has ended.
        /// </summary>
        public event Action<GameResult> OnRoundEnd;

        /// <summary>
        /// Occurs when next round has ranned.
        /// </summary>
        public event Action OnNextRound;

        /// <summary>
        /// Gets the current round number.
        /// </summary>
        /// <value>The current round.</value>
        public int CurrentRound { get; private set; }

        /// <summary>
        /// Initializes this instance immediately after completion of all dependency injection.
        /// </summary>
        public void OnInjected()
        {
            LevelMarkers.FinishArea.OnFinished += OnFinishedHandler;
        }

        /// <summary>
        /// Called when the attached Behaviour is destroying.
        /// </summary>
        private void OnDestroy()
        {
            LevelMarkers.FinishArea.OnFinished -= OnFinishedHandler;
        }

        /// <summary>
        /// Adds the alive player unit in the scene to to account for the loss.
        /// </summary>
        /// <param name="unit">The alive player's unit.</param>
        public void AddAlivePlayerUnit(Unit unit)
        {
            _aliveUnits.Add(unit);
        }

        /// <summary>
        /// Deletes a deceased unit from the alived units list and ends the round if there are no survivors.
        /// </summary>
        /// <param name="unit">The alive player's unit.</param>
        public void RemoveAlivePlayerUnit(Unit unit)
        {
            _aliveUnits.Remove(unit);

            if (_currentGameResult.HasValue)
                return;

            if (_aliveUnits.Count == 0)
            {
                _currentGameResult = GameResult.Lose;
                OnRoundEnd?.Invoke(_currentGameResult.Value);
            }
        }

        /// <summary>
        /// Runs the next round.
        /// </summary>
        public void RunNextRound()
        {
            _aliveUnits.Clear();
            _currentGameResult = null;
            CurrentRound++;
            OnNextRound?.Invoke();
        }

        /// <summary>
        /// Called when player's unit has crossed finish-area.
        /// </summary>
        /// <param name="unit">The player's unit.</param>
        private void OnFinishedHandler(Unit unit)
        {
            if (_currentGameResult.HasValue)
                return;

            if (unit.IsAlive)
            {
                _currentGameResult = GameResult.Win;
                OnRoundEnd?.Invoke(_currentGameResult.Value);
            }
        }
    }
}
