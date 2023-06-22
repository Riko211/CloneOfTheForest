using Infrastructure.Services;
using UI;
using UnityEngine;

namespace Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string InitialPointTag = "InitialPoint";
        private const string HeroPath = "Hero/hero";
        private readonly GameStateMachine _gameStateMachine;
        private LoadingScreen _loadingScreen;
        private readonly AllServices _services;

        public LoadLevelState(AllServices services, LoadingScreen loadingScreen, GameStateMachine gameStateMachine)
        {
            _services = services;
            _gameStateMachine = gameStateMachine;
            _loadingScreen = loadingScreen;
        }

        public void Enter(string sceneName)
        {
            _loadingScreen.ShowLoadingScreen();
            _services.Single<SceneLoader>().Load(sceneName, onLoaded);
        }
        
        private void onLoaded()
        {
            var InitialPoint = GameObject.FindWithTag(InitialPointTag);
            GameObject hero = Instantiate(HeroPath, at: InitialPoint.transform.position);

            _gameStateMachine.Enter<GameFlowState>();
        }

        private static GameObject Instantiate(string path)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab);
        }
        private static GameObject Instantiate(string path, Vector3 at)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab, at, Quaternion.identity);
        }
        
        public void Exit()
        {
            _loadingScreen.HideLoadingScreen();
        }
    }
}