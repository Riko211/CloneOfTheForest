using Infrastructure.Services;
using UI;
using UnityEngine;

namespace Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string InitialPointTag = "InitialPoint";

        private readonly GameStateMachine _gameStateMachine;
        private LoadingScreen _loadingScreen;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _services;
        private readonly GameFactory _gameFactory;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, GameFactory gameFactory, LoadingScreen loadingScreen)
        {
            _gameStateMachine = gameStateMachine;
            _loadingScreen = loadingScreen;
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;          
        }

        public void Enter(string sceneName)
        {
            _loadingScreen.ShowLoadingScreen();
            _sceneLoader.Load(sceneName, onLoaded);
        }
        
        private void onLoaded()
        {
            var InitialPoint = GameObject.FindWithTag(InitialPointTag);
            _gameFactory.CreateHero(InitialPoint);
            _gameFactory.CreateTarget();
            Cursor.lockState = CursorLockMode.Locked;

            _gameStateMachine.Enter<GameFlowState>();
        }
        
        public void Exit()
        {
            _loadingScreen.HideLoadingScreen();
        }
    }
}