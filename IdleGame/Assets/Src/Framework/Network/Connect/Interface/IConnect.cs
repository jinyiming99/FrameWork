namespace GameFrameWork.Network.Interface
{
    public interface IConnect
    {
        MessageConstDefine.ConnectCallBack ConnectCallBack { set; }
        MessageConstDefine.ReveiceCallBack ReveiceCallback { set; }
        
        MessageConstDefine.ErrorCallBack ErrorCallBack{ set; } 
        MessageConstDefine.CloseCallBack CloseCallBack{ set; }
        
        public string IP { set; get; }
        public int Port { set; get; }
        
        bool IsConnected { get; }
        void ConnectAsync();

        void DisConnect();
        
        void SendAsync(byte[] data,int length,int offer);

        void Dispose();

    }
}