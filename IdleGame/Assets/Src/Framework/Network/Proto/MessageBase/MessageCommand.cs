namespace GameFrameWork.Network.MessageBase
{
    public enum MessageCommand
    {
        /// <summary>
        /// 确认消息
        /// </summary>
        Ack = 1,
        /// <summary>
        /// PB消息
        /// </summary>
        ProtoBuf_Message = 2,
        /// <summary>
        /// 协议心跳
        /// </summary>`
        Heart =3 ,
    }
}