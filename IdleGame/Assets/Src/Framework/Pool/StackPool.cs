using System.Collections.Generic;

namespace GameFrameWork.Pool
{
    public class StackPool<T> 
    {
        private Stack<T> m_stack;

        public StackPool(int length)
        {
            m_stack = new Stack<T>(length);
        }

        public StackPool()
        {
            m_stack = new Stack<T>(8);
        }

        public int GetLength()
        {
            return m_stack.Count;
        }

        public virtual T Pop()
        {
            if (m_stack.Count > 0)
                return m_stack.Pop();
            return default;
        }

        public void Push(T t)
        {
            if (!m_stack.Contains(t))
                m_stack.Push(t);
        }

        public void Release()
        {
            if (m_stack.Count > 0)
            {
                m_stack.Clear();
            }
        }
    }
}