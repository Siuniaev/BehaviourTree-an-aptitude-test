using Assets.Scripts.DI;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// The main class of the game.
    /// </summary>    
    /// <seealso cref="IGame" />
    internal class Game : MonoBehaviour, IGame
    {
        // Everything the game needs.
        [Injection] private ILevelMarkersProvider LevelMarkers { get; set; }
        [Injection] private IUnitsCollector UnitsCollector { get; set; }
        [Injection] private IPlayerUnitsSpawner PlayerUnitsSpawner { get; set; }
        [Injection] private IEnemiesSpawner EnemiesSpawner { get; set; }
        [Injection] private IGameReferee GameReferee { get; set; }
        [Injection] private ICameraMain CameraMain { get; set; }
        [Injection] private IObjectsPool ObjectsPool { get; set; }
        [Injection] private IHUD HUD { get; set; }

        /// <summary>
        /// Is called when the script instance is being loaded.        
        /// </summary>
        private void Awake()
        {
            // Automatically exit the app when the back button is pressed.
            Input.backButtonLeavesApp = true;
        }

        /// <summary>
        /// Starts the game.
        /// </summary>        
        public void StartGame() => GameReferee.RunNextRound();
    }
}
