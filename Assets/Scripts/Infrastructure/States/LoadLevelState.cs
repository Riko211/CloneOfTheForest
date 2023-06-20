using Infrastructure.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.States
{
    public class LoadLevelState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
        }

        public GameStateMachine GameStateMachine { get; }
        public SceneLoader SceneLoader { get; }

        public void Enter()
        {
            //_sceneLoader.Load("GameScene");
        }

        public void Exit()
        {
        }
    }
}