namespace GameFrameWork.Network.Interface
{
    public interface INetworkWorker
    {
        int GetID();
        void Release();
        
    }
}