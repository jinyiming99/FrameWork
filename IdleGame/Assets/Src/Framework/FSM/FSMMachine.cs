using System.Collections.Generic;

namespace GameFrameWork
{
    public interface IFSMMachine
    {
        void Update();

        void Release();
    }
    /// <summary>
    /// 状态机
    /// </summary>
    /// <typeparam name="T">状态的数据</typeparam>
    /// <typeparam name="U">状态类型</typeparam>
    /// <typeparam name="Enum">状态枚举</typeparam>
    public class FSMMachine<U, Enum> :IFSMMachine
            where U : class, IState 
    {        
        /// <summary>
        /// 状态表
        /// </summary>
        protected Dictionary<Enum, U> m_dic = new Dictionary<Enum, U>();
        /// <summary>
        /// 当前状态
        /// </summary>
        protected U m_curState;
        /// <summary>
        /// 下一个要切换的状态
        /// </summary>
        protected U m_nextState;
        /// <summary>
        /// 之前的状态
        /// </summary>
        protected U m_preState;
        /// <summary>
        /// 要传的数据
        /// </summary>
        protected object[] m_args;

        protected Enum m_stateType;

        public Enum GetCurType()
        {
            return m_stateType;
        }
        
        public FSMMachine()
        {

        }
        /// <summary>
        /// 更新
        /// </summary>
        public virtual void Update()
        {
            Change(m_args);
            if (m_curState != null)
                m_curState.Update();
        }

        public virtual void Release()
        {
            if (m_curState != null)
                m_curState.Release();
        }
        /// <summary>
        /// 添加状态
        /// </summary>
        /// <param name="stateEnum"></param>
        /// <param name="state"></param>
        public void AddState(Enum stateEnum,U state)
        {
            if (!m_dic.ContainsKey(stateEnum))
                m_dic.Add(stateEnum, state);
        }
        /// <summary>
        /// 外部调用的切换状态
        /// </summary>
        /// <param name="state"></param>
        /// <param name="args"></param>
        public void ChangeState(Enum state, params object[] args)
        {
            m_args = args;
            if (!m_dic.TryGetValue(state,out m_nextState))
            {
                DebugTools.DebugHelper.Log(()=>"没有state  = " + state.ToString());
            }
            m_stateType = state;
        }
        /// <summary>
        /// 切换状态
        /// </summary>
        /// <param name="args"></param>
        protected virtual void Change(params object[] args)
        {
            if (m_nextState != null)
            {
                if (m_curState != null && m_curState.CanRelease( args) && m_nextState.CanEnter(args))
                {
                    m_curState.Release( args);
                    m_preState = m_curState;
                    m_curState = m_nextState;
                    m_nextState = null;
                    m_curState.Enter( args);
                }
                else if (m_curState == null)
                {
                    m_curState = m_nextState;
                    m_nextState = null;
                    m_curState.Enter(args);
                }
            }
        }
        /// <summary>
        /// 获取当前状态，如果没有就返回下个状态
        /// </summary>
        /// <returns></returns>
        public U GetState()
        {
            if (m_curState != null)
                return m_curState;
            else
                return m_nextState;
        }
        /// <summary>
        /// 获取下个状态
        /// </summary>
        /// <returns></returns>
        public U GetNextState()
        {
            return m_nextState;
        }

        public U GetPreState()
        {
            return m_preState;
        }
    }
}