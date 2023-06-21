using Infrastructure.Services;
using UnityEngine;

namespace Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly AllServices _services;

        public LoadLevelState(GameStateMachine gameStateMachine, AllServices services)
        {
            _gameStateMachine = gameStateMachine;
            _services = services;
        }

        public void Enter(string sceneName)
        {
            _services.Single<SceneLoader>().Load(sceneName, onLoaded);
        }
        
        private void onLoaded()
        {
            GameObject hero = Instantiate("Hero/hero");
        }

        private static GameObject Instantiate(string path)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab);
        }
        
        public void Exit()
        {
        }
    }
}