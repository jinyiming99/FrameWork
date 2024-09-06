using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Threading.Tasks;
using GameFrameWork.DebugTools;
using GameFrameWork.Network.Interface;
using GameFrameWork.Network.MessageBase;

namespace GameFrameWork.Network
{
    public class NetworkWorker :INetworkWorker
    {
        private IConnect m_connect;

        private int m_id = NetworkCounter.GetCount();

        public INetworkCallback _NetworkCallback;

        private DataSegment _dataSegment = new DataSegment();

        private bool _isConnecting = false;
        public bool IsConnecting => _isConnecting;
        internal NetworkWorker(IConnect connect)
        {
            m_connect = connect;
            m_connect.ReveiceCallback = OnReveiceData;
            m_connect.ErrorCallBack = OnErrorCallback;
            m_connect.CloseCallBack = OnClose;
            m_connect.ConnectCallBack = OnConnectCallBack;
            _isConnecting = false;
        }

        public NetworkConnectStatusEnum CheckConnectStatus()
        {
            if (m_connect == null)
                return NetworkConnectStatusEnum.None;

            if (m_connect.IsConnected)
            {
                return NetworkConnectStatusEnum.Connected;
            }
            else
            {
                return NetworkConnectStatusEnum.Disconnect;
            }
        }

        private void OnClose()
        {
            Disconnect();
        }

        public void StartConnect(INetworkCallback callback)
        {
            try
            {
                _NetworkCallback = callback;
                m_connect.ConnectAsync();
            }
            catch (Exception e)
            {
                DebugTools.DebugHelper.LogError(e.ToString());
                throw;
            }
            
        }

        public void Disconnect()
        {
            //if (m_connect.IsConnected)
            //{
                m_connect.Dispose();
                _NetworkCallback.closeCallback?.Invoke();
                _isConnecting = false;
            //}
        }
        public void ReConnect()
        {
            
        }

        void OnConnectCallBack(NetworkErrorEnum status)
        {
            if (status == NetworkErrorEnum.Success)
                _isConnecting = true;
            _NetworkCallback.connectCallback?.Invoke(status);
        }
        
        
        public void SendAsync(DataSegment data)
        {
            m_connect.SendAsync(data.m_data,data.Length,0);
        }

        public void SendMessageAsync(byte[] datas, int length)
        {
            MessageBase.MessageBase data = new MessageBase.MessageBase();
            data.m_data = datas;
            data.m_length = length;
            //data.m_cmd = (int)MessageCommand.ProtoBuf_Message;
            //data.m_messageID = @enum;
            SendAsync(data.GetData());
        }

        void OnReveiceData(byte[] data , int length)
        {
//            DebugHelper.Log($"OnReveiceData data {length}");
            _dataSegment.WriteArray(data,length);
            DebugTools.DebugHelper.Log(() =>
            {
                string str = string.Empty;
                for (int index = 0 ;index < length;index++)
                {
                    str += data[index].ToString() + " ";
                }
            
                return str;
            });
            while (MessageBase.MessageBase.TryGetMessage(_dataSegment, out var msg))
            {
                new Task(() =>
                {
                    _NetworkCallback.receiveMessageCallback?.Invoke(msg);
                }).Start();
                _dataSegment.MoveData();
            }
        }

        void OnErrorCallback(ConnectErrorStatus status,string reason)
        {
            _NetworkCallback.errorCallback?.Invoke(status,reason);
        }


        public int GetID()
        {
            return m_id;
        }

        public void Release()
        {
            if (m_connect.IsConnected)
                m_connect.Dispose();
        }
    }
}