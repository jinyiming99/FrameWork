using System.Net.Sockets;

namespace GameFrameWork.Network
{
    public static class MessageConstDefine
    {
        public static readonly int MESSAGE_ID_SIZE = 2;
        public static readonly int MESSAGE_CMD_SIZE = 2;
        public static readonly int MESSAGE_INDEX_SIZE = 4;
        public static readonly int MESSAGE_LENGTH_SIZE = 4;
        
        public delegate void ConnectCallBack(NetworkErrorEnum status);
        public delegate void ReveiceCallBack(byte[] datas,int length);
        public delegate void ReveiceMessageBaseCallBack(MessageBase.MessageBase msg);
        public delegate void ErrorCallBack(ConnectErrorStatus error,string msg);
        public delegate void CloseCallBack();
    }
}