using System;
using System.Collections.Generic;
using GameFrameWork.Network.Client;
using GameFrameWork.Network.MessageBase;
using GameFrameWork.Network.WebSocket;
using UnityEngine;

namespace GameFrameWork.Network
{
    public class NetworkComponent : MonoBehaviour
    {
        private Dictionary<NetworkConfig,NetworkWorker> _networkWorkers = new Dictionary<NetworkConfig, NetworkWorker>();
        private MessageDistribute _messageDistribute;
        public MessageDistribute MessageDistribute => _messageDistribute;
        private void Awake()
        {
            var types = Util.AssemblyTool.FindTypeBase(typeof(MessageDistribute));
            if (types is not null && types.Length > 0)
            {
                foreach (var type in types)
                {
                    if (!type.IsAbstract && type.IsSubclassOf(typeof(MessageDistribute)))
                    {
                        var obj = Activator.CreateInstance(type);
                        if (obj is null)
                            continue;
                        _messageDistribute = obj as MessageDistribute;
                        if (_messageDistribute is not null)
                            _messageDistribute.CreateMessageDic();
                        return;
                    }
                }
            }
        }

        public NetworkWorker CreateConnect(NetworkConfig config)
        {
            NetworkWorker worker = null;
            switch (config.networkObjectType)
            {
                case NetworkObjectType.WebSocket:
                    worker = WebSocketTcpClient.CreateWebClient(config.ip, config.port, config.webSocketSafeType);
                    break;
                case NetworkObjectType.TcpIP:
                    worker = TcpClient.CreateTcpConnect(config.ip, config.port);
                    break;
            }

            if (worker != null)
            {
                _networkWorkers.Add(config,worker);
            }
            return worker;
        }

        public void Release()
        {
            foreach (var networkWorker in _networkWorkers)
            {
                networkWorker.Value.Release();
            }
        }
    }
}