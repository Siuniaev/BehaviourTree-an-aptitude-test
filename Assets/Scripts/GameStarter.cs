using Assets.Scripts.DI;
using Assets.Scripts.UI;

namespace Assets.Scripts
{
    /// <summary>
    /// Configures and starts the game.
    /// </summary>
    /// <seealso cref="Singleton{GameStarter}" />
    internal sealed class GameStarter : Singleton<GameStarter>
    {
        private DIContainer container;

        /// <summary>
        /// Is called on the frame when a script is enabled just before any of the Update methods are called the first time.
        /// </summary>
        private void Start()
        {
            // Here we can configure the game as we like by changing the dependencies.            

            container = new DIContainer();
            container.RegisterSingleton<IGame, Game>();
            container.RegisterSingleton<IGameReferee, GameReferee>();
            container.RegisterSingleton<ILevelMarkersProvider, LevelMarkersProvider>();
            container.RegisterSingleton<IPlayerUnitsSpawner, PlayerUnitsSpawner>();
            container.RegisterSingleton<IEnemiesSpawner, EnemiesSpawner>();
            container.RegisterSingleton<IUnitsCollector, UnitsCollector>();
            container.RegisterSingleton<ICameraMain, CameraFollowedUnit>();
            container.RegisterSingleton<IObjectsPool, ObjectsPool>();
            container.RegisterSingleton<IHUD, HUD>();

            var game = container.Resolve<IGame>();
            game.StartGame();
        }
    }
}