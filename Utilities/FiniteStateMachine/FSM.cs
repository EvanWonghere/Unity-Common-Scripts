namespace Utilities.FiniteStateMachine
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// 状态机类，管理状态的切换和执行
    /// </summary>
    public class FSM
    {
        private readonly Dictionary<Type, IState> _states = new();
        private IState _currentState;

        /// <summary>
        /// 添加状态到状态机
        /// </summary>
        public void AddState<T>(T state) where T : IState
        {
            var type = typeof(T);
            if (_states.TryAdd(type, state)) return;
            Debug.LogWarning($"State {type.Name} already exists.");
        }

        /// <summary>
        /// 切换到指定类型的状态
        /// </summary>
        public void SwitchState<T>() where T : IState
        {
            var type = typeof(T);
            if (_states.TryGetValue(type, out var state))
            {
                _currentState?.OnExit();
                _currentState = state;
                _currentState.OnEnter();
            }
            else
            {
                Debug.LogWarning($"State {type.Name} does not exist.");
            }
        }

        /// <summary>
        /// 执行当前状态的逻辑
        /// </summary>
        public void ExecuteState()
        {
            _currentState?.OnExecute();
        }
    }
}