using Assets.Scripts.DI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Collects and gives access to all units in the scene.
    /// </summary>    
    /// <seealso cref="IUnitsCollector" />
    /// <seealso cref="IInitiableOnInjecting" />
    internal class UnitsCollector : MonoBehaviour, IUnitsCollector, IInitiableOnInjecting
    {
        [Injection] private IPlayerUnitsSpawner PlayerUnitsSpawner { get; set; }
        [Injection] private IEnemiesSpawner EnemiesSpawner { get; set; }
        [Injection] private IGameReferee GameReferee { get; set; }

        private Dictionary<Team, List<Unit>> _allUnits = new Dictionary<Team, List<Unit>>()
        {
            { Team.Enemies, new List<Unit>()},
            { Team.Player, new List<Unit>()}
        };

        /// <summary>
        /// Initializes this instance immediately after completion of all dependency injection.
        /// </summary>
        public void OnInjected()
        {
            PlayerUnitsSpawner.OnUnitSpawned += OnUnitSpawnedHandler;
            EnemiesSpawner.OnUnitSpawned += OnUnitSpawnedHandler;
            GameReferee.OnRoundEnd += OnRoundEndHandler;
            GameReferee.OnNextRound += OnNextRoundHandler;
        }

        /// <summary>
        /// Called when the attached Behaviour is destroying.
        /// </summary>
        private void OnDestroy()
        {
            PlayerUnitsSpawner.OnUnitSpawned -= OnUnitSpawnedHandler;
            EnemiesSpawner.OnUnitSpawned -= OnUnitSpawnedHandler;
            GameReferee.OnRoundEnd -= OnRoundEndHandler;
            ClearAllCollectedUnits();
        }

        /// <summary>
        /// Gets the units by given team.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <returns>The units.</returns>
        public IReadOnlyCollection<Unit> GetUnitsByTeam(Team team)
        {
            return GetUnitsOfTeam(team);
        }

        /// <summary>
        /// Called when next round has started.
        /// </summary>
        private void OnNextRoundHandler()
        {
            // Remove current units.
            EnemiesSpawner.StopSpawningEnemies();
            DestroyAllCollectedUnits();

            // Start spawning new units.
            PlayerUnitsSpawner.SpawnUnits();
            EnemiesSpawner.StartSpawningEnemies(GameReferee.CurrentRound - 1);
        }

        /// <summary>
        /// Destroys all collected units (removes to pool).
        /// </summary>
        private void DestroyAllCollectedUnits()
        {
            foreach (var keyValuePair in _allUnits)
            {
                foreach (var unit in keyValuePair.Value)
                {
                    unit.OnUnitDie -= OnUnitDieHandler;
                    unit.DestroyAsPoolableObject();
                }

                keyValuePair.Value.Clear();
            }
        }

        /// <summary>
        /// Called when the new unit has spawned.
        /// </summary>
        /// <param name="unit">The new unit.</param>
        /// <exception cref="System.ArgumentNullException">unit</exception>
        private void OnUnitSpawnedHandler(Unit unit)
        {
            if (unit == null)
                throw new ArgumentNullException(nameof(unit));

            GetUnitsOfTeam(unit.Team)?.Add(unit);
            unit.transform.SetParent(transform);
            unit.OnUnitDie += OnUnitDieHandler;

            if (unit.Team == Team.Player)
                GameReferee.AddAlivePlayerUnit(unit);
        }

        /// <summary>
        /// Called when the unit has died.
        /// </summary>
        /// <param name="unit">The died unit.</param>
        /// <exception cref="System.ArgumentNullException">unit</exception>
        private void OnUnitDieHandler(Unit unit)
        {
            if (unit == null)
                throw new ArgumentNullException(nameof(unit));

            GetUnitsOfTeam(unit.Team)?.Remove(unit);
            unit.OnUnitDie -= OnUnitDieHandler;

            if (unit.Team == Team.Player)
                GameReferee.RemoveAlivePlayerUnit(unit);
        }

        /// <summary>
        /// Gets the units collection of the specified team.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <returns>The units collection.</returns>
        private List<Unit> GetUnitsOfTeam(Team team)
        {
            _allUnits.TryGetValue(team, out var collection);
            return collection;
        }

        /// <summary>
        /// Called when the round has ended.
        /// </summary>
        /// <param name="result">The game result.</param>
        private void OnRoundEndHandler(GameResult result)
        {
            EnemiesSpawner.StopSpawningEnemies();

            foreach (var keyValuePair in _allUnits)
            {
                foreach (var unit in keyValuePair.Value)
                    unit.SetBehaviour(new UnitBehaviourEndGame());
            }
        }

        /// <summary>
        /// Clears all units collections (without destroying units).
        /// </summary>
        private void ClearAllCollectedUnits()
        {
            foreach (var keyValuePair in _allUnits)
            {
                foreach (var unit in keyValuePair.Value)
                    unit.OnUnitDie -= OnUnitDieHandler;

                keyValuePair.Value.Clear();
            }
        }
    }
}
