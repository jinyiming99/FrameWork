using System;
using System.Collections.Generic;

namespace GameFrameWork.Pool
{
    public static class ClassPool<T> where T: class,new()
    {
        private static Stack<T> m_stack = new Stack<T>();

        public static T Spawn()
        {
            if (m_stack.Count > 0)
            {
                return m_stack.Pop();
            }

            return new T();
        }

        public static void Despawn(T obj)
        {
            m_stack.Push(obj);
        }

        public static void Clear()
        {
            m_stack.Clear();
        }
    }
}