using System.Collections.Generic;

namespace Expression
{
    public interface IPool
    {
        void Clear();
    }
    public class ClassPool<T> :IPool where T:class
    {
        private Stack<T> _stack = new Stack<T>();
        public int Count => _stack.Count;
        public void Push(T value)
        {
            _stack.Push(value);
        }
        public T Pop()
        {
            if (_stack.Count == 0)
                return default;
            return _stack.Pop();
        }
        public void Clear()
        {
            _stack.Clear();
        }
    }
    public static class ClassPoolManager
    {
        private static Dictionary<System.Type, IPool> _pools = new Dictionary<System.Type, IPool>();
        public static ClassPool<T> GetPool<T>() where T:class
        {
            if (_pools.TryGetValue(typeof(T), out var pool))
            {
                return (ClassPool<T>)pool;
            }
            else
            {
                var newPool = new ClassPool<T>();
                _pools.Add(typeof(T), newPool);
                return newPool;
            }
        }
        public static void Clear()
        {
            foreach (var pool in _pools.Values)
            {
                pool.Clear();
            }
            _pools.Clear();
        }
    } 
}