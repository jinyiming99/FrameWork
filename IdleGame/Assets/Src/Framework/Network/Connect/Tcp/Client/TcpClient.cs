using System;
using System.Net;
using System.Net.Sockets;

namespace GameFrameWork.Network.Client
{
    public class TcpClient 
    {
        public static NetworkWorker CreateTcpConnect(string ip, int port)
        {
            var connect = CreateConnect();
            connect.IP = ip;
            connect.Port = port;
            var user = new NetworkWorker(connect);
            return user;
        }
        
        private static Socket CreateSocket()
        {
            Socket socket = null;
            AddressFamily af = CheckFamily();
            try
            {
                socket = new Socket(af, SocketType.Stream, ProtocolType.Tcp);
            }
            catch
            {
                if (af == AddressFamily.InterNetworkV6)
                    af = AddressFamily.InterNetwork;
                else if (af == AddressFamily.InterNetwork)
                    af = AddressFamily.InterNetworkV6;
                try
                {
                    socket = new Socket(af, SocketType.Stream, ProtocolType.Tcp);
                }
                catch(Exception e)
                {
                    var str = e.ToString();
                    DebugTools.DebugHelper.LogError($" create socket failed ,e ={str}");
                    throw new NetworkException(NetworkErrorEnum.Socket_Create_Failed, str);
                }
            }
            return socket;
        }
        
        private static AddressFamily CheckFamily()
        {
            AddressFamily af = AddressFamily.InterNetwork;

            try
            {
                IPAddress[] address = Dns.GetHostAddresses(Dns.GetHostName());
                if (address[0].AddressFamily == AddressFamily.InterNetworkV6)
                    af = AddressFamily.InterNetworkV6;
            }
            catch (Exception e)
            {

            }
            return af;
        }
        
        public static TcpConnect CreateConnect()
        {
            TcpConnect connect = new TcpConnect();
            connect.m_socket = CreateSocket();
            return connect;
        }
    }
}