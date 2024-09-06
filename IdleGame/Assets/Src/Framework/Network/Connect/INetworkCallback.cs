namespace GameFrameWork.Network
{
    public interface INetworkCallback
    {
        public MessageConstDefine.ErrorCallBack errorCallback { get; }
        
        public MessageConstDefine.ReveiceMessageBaseCallBack receiveMessageCallback {  get; }

        public MessageConstDefine.ConnectCallBack connectCallback { get; }
        
        public MessageConstDefine.CloseCallBack closeCallback { get; }
    }
}