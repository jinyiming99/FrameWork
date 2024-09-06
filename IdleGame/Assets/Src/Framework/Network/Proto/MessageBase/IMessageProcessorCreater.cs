namespace GameFrameWork.Network.MessageBase
{
    public interface IMessageProcessorCreater
    {
        IMessageProcessor CreateMessage(MessageBase msgBase);

        void Release(IMessageProcessor u);
    }
}