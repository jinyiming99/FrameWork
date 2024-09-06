namespace GameFrameWork.Network.MessageBase
{
    public class MessageStruct<T> where T:new()
    {

        public T message;
        public int ack;

        public MessageStruct()
        {
            message = new T();
            ack = 0;
        }
        
    }
}