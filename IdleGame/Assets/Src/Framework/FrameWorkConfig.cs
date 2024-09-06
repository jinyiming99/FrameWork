using System;
using NaughtyAttributes;
using UnityEngine;
using GameFrameWork.Network;
using GameFrameWork.Network.Client;
using GameFrameWork.Network.WebSocket;
namespace GameFrameWork
{
    public enum NetworkObjectType
    {
        TcpIP,
        WebSocket,
    }
    [Serializable]
    public class NetworkConfig
    {
        public bool useNetwork = false;

        [SerializeField]
        internal NetworkObjectType networkObjectType = NetworkObjectType.TcpIP;

        [SerializeField]
        [ShowIf("isWebSocket")]
        internal WebSocketTcpConnect.WebSocketSafeType webSocketSafeType = WebSocketTcpConnect.WebSocketSafeType.WS;
        [SerializeField]
        internal string ip = "127.0.0.1";
        [SerializeField]
        internal int port = 8080;
        internal bool isWebSocket => networkObjectType == NetworkObjectType.WebSocket;
    }
    [DisallowMultipleComponent]
    public class FrameWorkConfig : MonoBehaviour
    {
        [SerializeField]
        public NetworkConfig[] networkConfig;
        
    }
}