using Infrastructure.Services;
using Infrastructure.States;
using System;
using UnityEngine;

namespace Infrastructure.States
{
    public class BootState : IState
    {
        private const string Initial = "Initial";
        private readonly AllServices _services;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;

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
            _sceneLoader.Load(Initial, onLoaded: EnterLoadLevel);
        }

        private void EnterLoadLevel()
        {
            throw new NotImplementedException();
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