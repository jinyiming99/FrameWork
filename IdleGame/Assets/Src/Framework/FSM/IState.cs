namespace GameFrameWork
{
    /// <summary>
    /// 状态机状态的接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IState
    {
        /// <summary>
        /// 是否可以进入状态
        /// </summary>
        /// <param name="data"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        bool CanEnter(params object[] args);
        /// <summary>
        /// 进入状态
        /// </summary>
        /// <param name="data"></param>
        /// <param name="args"></param>
        void Enter(params object[] args);
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="data"></param>
        void Update();
        /// <summary>
        /// 是否可以释放状态
        /// </summary>
        /// <param name="data"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        bool CanRelease(params object[] args);
        /// <summary>
        /// 释放状态
        /// </summary>
        /// <param name="data"></param>
        /// <param name="args"></param>
        void Release(params object[] args);
    }
}


