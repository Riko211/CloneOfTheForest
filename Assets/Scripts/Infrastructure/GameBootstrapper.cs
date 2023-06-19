using Infrastructure.States;
using UnityEngine;

namespace Infrastructure
{
    public class GameBootstrapper : MonoBehaviour
    {
        public GameBootstrapper Instance;
        public GameStateMachine StateMachine;

        private void Awake()
        {
            Instance = this;
            StateMachine = new GameStateMachine();
            StateMachine.Enter<BootstrapState>();


            DontDestroyOnLoad(this);
        }
    }
}