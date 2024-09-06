using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using GameFrameWork.Network.Interface;

namespace GameFrameWork.Network
{
    public enum ConnectErrorStatus
    {
        Send_Error,
        Reveice_Error,
        Connect_Error,
        Connecting_Error,
    }

    
    public class TcpConnect : IConnect
    {
        internal Socket m_socket;
        
        public string IP { set; get; }

        public int Port { get; set; }

        private bool m_isConnected;

        private MessageConstDefine.ConnectCallBack m_connectCallback;
        private MessageConstDefine.ReveiceCallBack m_reveiceCallback;
        private MessageConstDefine.CloseCallBack m_closeCallback;
        private MessageConstDefine.ErrorCallBack m_errorCallback;

        public MessageConstDefine.ConnectCallBack ConnectCallBack
        {
            set => m_connectCallback = value;
        }

        public MessageConstDefine.ReveiceCallBack ReveiceCallback
        {
            set => m_reveiceCallback = value;
        }

        public MessageConstDefine.CloseCallBack CloseCallBack
        {
            set => m_closeCallback = value;
        }

        public MessageConstDefine.ErrorCallBack ErrorCallBack
        {
            set => m_errorCallback = value;
        }
        
        private byte[] m_receiveData = new byte[1024 * 1024];

        public static TcpConnect CreateConnect(Socket socket)
        {
            TcpConnect connect = new TcpConnect();
            connect.m_socket = socket;
            if (socket.Connected)
                connect.m_isConnected = true;
            return connect;
        }
        
        internal TcpConnect()
        {
            
        }

        public void ConnectAsync()
        {
            try
            {
                m_socket.BeginConnect(IP, Port, (result)=>
                {
                    DebugTools.DebugHelper.Log(()=>$"connect result = {result.IsCompleted}");
                    m_socket.EndConnect(result);
                    if (result.IsCompleted)
                    {
                        ReceiveAsync();
                        m_connectCallback?.Invoke( NetworkErrorEnum.Success);
                        m_isConnected = true;
                    }
                    else
                    {
                        m_connectCallback?.Invoke(NetworkErrorEnum.Socket_Connect_Failed);
                    }
                }, null);
            }
            catch (Exception e)
            {
                m_connectCallback?.Invoke(NetworkErrorEnum.Socket_Connect_Failed);
            }
        }

        public void DisConnect()
        {
            Dispose();
        }

        public long GetHashCode()
        {
            if (m_socket != null)
                return m_socket.GetHashCode();
            return 0L;
        }

        public string GetMac()
        {
            if (m_socket != null)
            {
                var endpoint = (IPEndPoint) m_socket.RemoteEndPoint;
                return endpoint.Address.ToString();
            }

            return string.Empty;
        }

        public void Dispose()
        {
            if (m_socket != null)
            {
                m_socket.Shutdown(SocketShutdown.Both);
                m_socket.Close();
                m_socket = null;
                m_closeCallback?.Invoke();
            }

            m_isConnected = false;
        }
        
        public void SendAsync(byte[] data,int length,int offer)
        {
            if (m_socket != null)
            {
                int sendLength = 0;
                m_socket.BeginSend(data, offer, length, SocketFlags.None,out var sendError,  (iar) =>
                {
                    if (m_isConnected)
                    {
                        int size = m_socket.EndSend(iar, out var errorCode);
                        new Task(() =>
                        {
                            if (size == 0 || errorCode != SocketError.Success)
                            {
                                Dispose();
                                return;
                            }

                            if (size < length)
                            {
                                SendAsync(data, length, offer);
                            }
                        }).Start();
                        
                    }
                },null);
                if (sendError != SocketError.Success)
                {
                    Dispose();
                }
            }
        }

        public void BeginReceive()
        {
            ReceiveAsync();
        }

        public void ReceiveAsync()
        {
            if (m_socket != null)
            {
                m_socket.BeginReceive(m_receiveData, 0, m_receiveData.Length, SocketFlags.None, out var receiveErrorCode,(iar) =>
                {
                    ///这里是异步操作
                    if (!m_isConnected) 
                        return;
                    //进入工作线程
                    new Task(() =>
                    {
                        int size = m_socket.EndReceive(iar, out var errorCode);
                        if (size == 0 || errorCode != SocketError.Success)
                        {
                            Dispose();
                            m_errorCallback?.Invoke(ConnectErrorStatus.Reveice_Error,$"eroor code = {errorCode}");
                        }
                        else ///读取成功,丢回给上级
                        {
                            m_reveiceCallback?.Invoke(m_receiveData,size);
                            ReceiveAsync();
                        }
                    }).Start();
                    
                },null);
                
                if (receiveErrorCode != SocketError.Success)
                {
                    m_errorCallback?.Invoke(ConnectErrorStatus.Reveice_Error,$"eroor code = {receiveErrorCode}");
                    Dispose();
                }
            }
        }

        public int ReceiveSteamSize()
        {
            if (m_socket != null)
            {
                return m_socket.Available;
            }

            return 0;
        }

        public bool IsConnected => m_isConnected;
    }
}