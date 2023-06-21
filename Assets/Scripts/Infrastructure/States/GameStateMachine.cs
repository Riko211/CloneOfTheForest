using Infrastructure.Services;
using System;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Infrastructure.States
{
    public class GameStateMachine
    {
        private Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public GameStateMachine(AllServices services, LoadingScreen loadingScreen, ICoroutineRunner coroutineRunner)
        {
            _states = new Dictionary<Type, IExitableState>()
            {
                [typeof(BootState)] = new BootState(services, coroutineRunner, this),
                [typeof(LoadLevelState)] = new LoadLevelState(services, loadingScreen, this),
                [typeof(GameFlowState)] = new GameFlowState(this),
            };
        }
        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState: class, IPayloadedState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();

            TState state = GetState<TState>();
            _activeState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState
        {
            return _states[typeof(TState)] as TState;
        }
    }

}


