namespace GameFrameWork.Network
{
    public enum NetworkConnectStatusEnum
    {
        None,
        Connecting,
        Connected,
        Disconnect,
    }
    
    
    public enum NetworkErrorEnum
    {
        None,
        Success,
        NetworkManager_Null,
        WorkType_Error,
        Socket_Create_Failed,
        Socket_Connect_Failed,
    }
}