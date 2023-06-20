using Infrastructure.Services;
using Infrastructure.States;
using System;
using UnityEngine;

namespace Infrastructure.States
{
    public class BootState : IState
    {
        private const string GameSceneName = "GameScene";
        private readonly AllServices _services;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly GameStateMachine _gameStateMachine;
        private SceneLoader _sceneLoader;

        public BootState(AllServices services, ICoroutineRunner coroutineRunner, GameStateMachine gameStateMachine)
        {
            _services = services;
            _coroutineRunner = coroutineRunner;
            _gameStateMachine = gameStateMachine;
            LockFPS();
        }

        public void Enter()
        {
            RegisterServices();
            //_sceneLoader.Load(Initial, onLoaded: EnterLoadLevel);
            _sceneLoader = _services.Single<SceneLoader>();
            _sceneLoader.Load(GameSceneName, onLoaded: EnterLoadLevel);
        }

        private void EnterLoadLevel()
        {
            _gameStateMachine.Enter<LoadLevelState>();
        }

        private void RegisterServices()
        {
            _services.RegisterSingle<SceneLoader>(new SceneLoader(_coroutineRunner));
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