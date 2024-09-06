using System;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using GameFrameWork.Network.Interface;


namespace GameFrameWork.Network.WebSocket
{
    public class WebSocketTcpConnect : IConnect
    {
        public enum WebSocketSafeType
        {
            WS,
            WSS,
        }
        private ClientWebSocket _socket;
        public MessageConstDefine.ConnectCallBack ConnectCallBack { get; set; }
        public MessageConstDefine.ReveiceCallBack ReveiceCallback { get; set; }
        public MessageConstDefine.ErrorCallBack ErrorCallBack { get; set; }
        public MessageConstDefine.CloseCallBack CloseCallBack { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }
        public bool IsConnected { get; }

        private byte[] m_receiveData = new byte[1024 * 1024];

        private WebSocketSafeType _safeType = WebSocketSafeType.WS;

        public WebSocketSafeType SafeType
        {
            set => _safeType = value;
            get => _safeType;
        }
        public WebSocketTcpConnect()
        {
            _socket = new ClientWebSocket();
            _safeType = WebSocketSafeType.WS;
        }
        public WebSocketTcpConnect(WebSocketSafeType safeType)
        {
            _socket = new ClientWebSocket();
            _safeType = safeType;
        }

        public WebSocketTcpConnect(ClientWebSocket socket)
        {
            _socket = socket;
        }

        public async void ConnectAsync()
        {
            if (_socket != null && _socket.State != WebSocketState.Open)
            {
                switch (_safeType)
                {
                    case WebSocketSafeType.WS:
                        await _socket.ConnectAsync(new Uri($"ws://{IP}:{Port}/ws?userid={DeviceInfo.DeviceName}"), CancellationToken.None);
                        break;
                    case WebSocketSafeType.WSS:
                        await _socket.ConnectAsync(new Uri($"wss://{IP}:{Port}"), CancellationToken.None);
                        break;
                }

                if (_socket.State == WebSocketState.Open)
                {
                    ConnectCallBack?.Invoke(NetworkErrorEnum.Success);
                    BeginReceive();
                }
                else
                {
                    ConnectCallBack?.Invoke(NetworkErrorEnum.Socket_Connect_Failed);
                }
            }
        }

        public void DisConnect()
        {
            Dispose();
        }

        public async void BeginReceive()
        {
            if (_socket.State == WebSocketState.Open)
            {
                var result = await _socket.ReceiveAsync(new ArraySegment<byte>(m_receiveData), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    Dispose();
                }
                else
                {
                    ReveiceCallback?.Invoke(m_receiveData, result.Count);
                    BeginReceive();
                }
            }

        }

        public async void SendAsync(byte[] data, int length, int offer)
        {
            if (_socket.State == WebSocketState.Open)
            {
                await _socket.SendAsync(new ArraySegment<byte>(data), WebSocketMessageType.Binary, true,
                    CancellationToken.None);
            }
        }

        public void Dispose()
        {
            _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, String.Empty, CancellationToken.None);
            _socket.Dispose();
        }
    }
}