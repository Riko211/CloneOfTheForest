using Infrastructure.Services;
using Infrastructure.States;
using System;
using UnityEngine;

namespace Infrastructure.States
{
    public class BootState : IState
    {
        private const string Initial = "Boot";
        private readonly AllServices _services;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly GameStateMachine _gameStateMachine;
        private SceneLoader _sceneLoader;

        public BootState(AllServices services, ICoroutineRunner coroutineRunner, GameStateMachine gameStateMachine)
        {
            _services = services;
            _coroutineRunner = coroutineRunner;
            _gameStateMachine = gameStateMachine;

            RegisterServices();
            LockFPS();
        }

        public void Enter()
        {
            _sceneLoader = _services.Single<SceneLoader>();
            _sceneLoader.Load(Initial, onLoaded: EnterLoadLevel);
        }

        private void EnterLoadLevel()
        {
            _gameStateMachine.Enter<LoadLevelState, string>("GameScene");
        }

        private void RegisterServices()
        {
            _services.RegisterSingle<SceneLoader>(new SceneLoader(_coroutineRunner));
            _services.RegisterSingle<AssetProvider>(new AssetProvider());
            _services.RegisterSingle<GameFactory>(new GameFactory(_services.Single<AssetProvider>()));
            _services.RegisterSingle<EventManager>(new EventManager());

            GameObject inputServiceGameObject = _services.Single<GameFactory>().CreateInputSystem();
            _services.RegisterSingle<InputSystem>(inputServiceGameObject.GetComponent<InputSystem>());
        }

        private void LockFPS()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 144;
        }

        public void Exit()
        {
        }
    }
}