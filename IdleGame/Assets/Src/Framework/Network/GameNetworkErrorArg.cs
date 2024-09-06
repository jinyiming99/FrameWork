using GameFrameWork;
using GameFrameWork.Network;
using GameFrameWork.Pool;

namespace GameFrameWork.Network
{
    public class GameNetworkErrorArg : EventArg
    {
        public ConnectErrorStatus _connectErrorStatus;
        public string _errorMsg;

        public static GameNetworkErrorArg Create(ConnectErrorStatus error, string msg)
        {
            var arg = ClassPool<GameNetworkErrorArg>.Spawn();
            arg._connectErrorStatus = error;
            arg._errorMsg = msg;
            return arg;
        }

        public static void Release(GameNetworkErrorArg arg)
        {
            arg.Release();
            ClassPool<GameNetworkErrorArg>.Despawn(arg);
        }
        public override void Release()
        {
            _errorMsg = string.Empty;
        }

        public override int GetID()
        {
            return GetHashCode();
        }
    }
}