namespace Infrastructure.States
{
    internal class GameFlowState : IState
    {
        private readonly GameStateMachine _gameStateMachine;

        public GameFlowState(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        public void Enter()
        {
        }

        public void Exit()
        {
        }
    }
}