namespace GameFrameWork.Network.MessageBase
{
    /// <summary>
    /// 消息处理者
    /// </summary>
    public interface IMessageProcessor
    {
        void CreateMessage(MessageBase segment);

        void Processor();

        void ReleaseMessage();
    }
}