
using System;
using com.VRDemo.gamemessage;
using Game;
using Game.Network;
using GameFrameWork;
using GameFrameWork.DebugTools;
using GameFrameWork.Network;
using GameFrameWork.Network.Client;
using GameFrameWork.Network.MessageBase;
using GameFrameWork.Network.WebSocket;
using GameFrameWork.Pool;
using Google.Protobuf;
using NaughtyAttributes;
using UnityEngine;
using Any = Google.Protobuf.Any;

public class NetworkObject : INetworkCallback
{
    private static MessageDistribute GetDistribute()
    {
        return FrameWork.Instance.NetworkComponent.MessageDistribute;
    }

    public static NetworkObject Create(NetworkConfig config)
    {
        var network = ClassPool<NetworkObject>.Spawn();
        network._config = config;
        return network;
    }


    private NetworkWorker _networkWorker;
    private NetworkConfig _config;
    private Action _connectCallback;
    private Action _disconnectCallback;
    public void ConnectServer(Action action)
    {
        _connectCallback = action;
        _networkWorker = FrameWork.Instance.NetworkComponent.CreateConnect(_config);
        if (_networkWorker is null)
            return;

        GetDistribute();
        _networkWorker.StartConnect(this);
    }
    private void OnDisable()
    {
        DisConnect();
    }

    public void DisConnect()
    {
        var framework = FrameWork.Instance;
        if (framework is null || !_networkWorker.IsConnecting)
            return;
        _disconnectCallback?.Invoke();
        _networkWorker.Disconnect();
    }

    void ConnectObjectCallback(NetworkErrorEnum status)
    {
        DebugHelper.Log("连接状态：" + status);
        if (status == NetworkErrorEnum.Success)
        {
            _connectCallback?.Invoke();
        }
        else
        {
            DebugHelper.LogError($"连接失败：{status}");
        }
    }

    void ReceiveCallback(GameFrameWork.Network.MessageBase.MessageBase messagebase)
    {
        if (!GetDistribute().Find(messagebase.m_messageID, out var creater))
        {
            DebugHelper.LogError("未找到消息id：" + messagebase.m_messageID);
            return;
        }

        var msg = creater.CreateMessage(messagebase);
        FrameWork.Instance?.ActionWorker.MainPost(() =>
        {
            msg.Processor();
        });

        //Debug.Log("收到消息：" + messagebase.m_messageID);
    }

    void ErrorCallback(ConnectErrorStatus error, string msg)
    {
        _networkWorker.Disconnect();
        var arg = GameNetworkErrorArg.Create( error,msg );
        IdleGame.GameEvent.FireEventNow(this,arg);
        GameNetworkErrorArg.Release(arg);
        
        DebugHelper.LogError($"错误返回：{error} msg ={msg}");
    }

    void OnCloseCallback()
    {
        
    }

    public void SendMessage(com.VRDemo.gamemessage.MessageType msgId, IMessage message)
    {
        
        CommonMessage msg = ClassPool<CommonMessage>.Spawn();;
        msg.MsgType = msgId;
        msg.Body = new Any();
        var any = Google.Protobuf.WellKnownTypes.Any.Pack(message);

        msg.Body.TypeUrl = any.TypeUrl;//Google.Protobuf.WellKnownTypes.Any.Parser.ParseFrom(message.ToByteString());
        msg.Body.Value = any.Value;//Google.Protobuf.WellKnownTypes.Any.Parser.ParseFrom(message.ToByteString());

        _networkWorker.SendMessageAsync(msg.ToByteArray(), msg.CalculateSize());
        ClassPool<CommonMessage>.Despawn(msg);
    }

    public MessageConstDefine.ErrorCallBack errorCallback => ErrorCallback;
    public MessageConstDefine.ReveiceMessageBaseCallBack receiveMessageCallback => ReceiveCallback;
    public MessageConstDefine.ConnectCallBack connectCallback => ConnectObjectCallback;
    public MessageConstDefine.CloseCallBack closeCallback => OnCloseCallback;
}
