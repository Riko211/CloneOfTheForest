using Infrastructure.Services;
using Infrastructure.States;
using UnityEngine;

namespace Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        public GameBootstrapper Instance;
        public GameStateMachine StateMachine;

        private void Awake()
        {
            Instance = this;
            StateMachine = new GameStateMachine(new AllServices(), this);
            StateMachine.Enter<BootState>();


            DontDestroyOnLoad(this);
        }
    }
}