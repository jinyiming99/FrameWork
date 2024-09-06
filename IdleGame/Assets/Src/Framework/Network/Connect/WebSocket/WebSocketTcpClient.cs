namespace GameFrameWork.Network.WebSocket
{
    public class WebSocketTcpClient
    {
        public static NetworkWorker CreateWebClient(string ip, int port, WebSocketTcpConnect.WebSocketSafeType type)
        {
            WebSocketTcpConnect connect = new WebSocketTcpConnect(type);
            connect.IP = ip;
            connect.Port = port;
            var user = new NetworkWorker(connect);
            return user;
        }
    }
}