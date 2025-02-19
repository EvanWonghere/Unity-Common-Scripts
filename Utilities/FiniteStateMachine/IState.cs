namespace Utilities.FiniteStateMachine
{
    /// <summary>
    /// 状态接口，每个具体的状态都需要实现该接口
    /// </summary>
    public interface IState
    {
        void OnEnter();
        void OnExecute();
        void OnExit();
    }
}
