namespace GameFrameWork.Network.MessageBase
{
    public interface IMessageCreater<T>
    {
        T CreateMessage(MessageBase msgBase);
    }
}