using Infrastructure.Services;
using Infrastructure.States;
using UI;
using UnityEngine;

namespace Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] LoadingScreen _screen;

        public GameBootstrapper Instance;
        public GameStateMachine StateMachine;

        private void Awake()
        {
            Instance = this;

            GameObject loadingScreen = Resources.Load<GameObject>("UI/LoadingScreen");
            GameObject screen = Instantiate(loadingScreen);
            _screen = screen.GetComponent<LoadingScreen>();

            StateMachine = new GameStateMachine(new AllServices(), _screen, this);
            StateMachine.Enter<BootState>();


            DontDestroyOnLoad(this);
        }
    }
}