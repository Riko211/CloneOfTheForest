using Infrastructure.States;
using System;

namespace Infrastructure.States
{
    public class BootstrapState : IState
    {
        private GameStateMachine _stateMachine;

        public BootstrapState(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            RegisterServices();
        }

        private void RegisterServices()
        {

        }

        public void Exit()
        {

        }
    }
}