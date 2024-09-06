using GameFrameWork;
using GameFrameWork.Network;
using GameFrameWork.Pool;

namespace GameFrameWork.Network
{
    public class GameNetworkConnectArg : EventArg
    {
        public NetworkErrorEnum _status;

        public static GameNetworkConnectArg Create(NetworkErrorEnum error)
        {
            var arg = ClassPool<GameNetworkConnectArg>.Spawn();
            arg._status = error;
            return arg;
        }

        public static void Release(GameNetworkConnectArg arg)
        {
            arg.Release();
            ClassPool<GameNetworkConnectArg>.Despawn(arg);
        }
        public override void Release()
        {
            _status = NetworkErrorEnum.None;
        }

        public override int GetID()
        {
            return GetHashCode();
        }
    }
}