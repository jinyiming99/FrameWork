using GameFrameWork.Pool;

namespace GameFrameWork.Network.MessageBase
{
    public class MessageProcessorCreater<T> : IMessageProcessorCreater   where T : class,IMessageProcessor ,new()
    {
        public StackPool<T> _stackPool = new StackPool<T>();
        public T Creater()
        {
            var outData = _stackPool.Pop();
            if (outData == null)
                outData = new T();
            return outData;
        }

        public IMessageProcessor CreateMessage(MessageBase msgBase)
        {
            var outData = Creater();
            outData.CreateMessage(msgBase);
            return outData;
        }

        public void Release(IMessageProcessor t)
        {
            if (t == null)
                return;
            _stackPool.Push(t as T);
        }
        
    }
}