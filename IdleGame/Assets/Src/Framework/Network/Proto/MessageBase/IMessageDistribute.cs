namespace GameFrameWork.Network.MessageBase
{
    /// <summary>
    /// 消息分发者
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMessageDistribute
    {
        void CreateMessageDic();

        bool Find(int index,out IMessageProcessorCreater creater);
    }
}