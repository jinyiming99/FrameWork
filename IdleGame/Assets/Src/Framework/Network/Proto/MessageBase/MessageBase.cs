
using com.VRDemo.gamemessage;
using Google.Protobuf;

namespace GameFrameWork.Network.MessageBase
{
    public class MessageBase
    {
        public int m_messageID ;
        //public short m_cmd ;
        //public int m_ack;
        public int m_length;
        public byte[] m_data;

        public MessageBase()
        {
            //m_ack = GetSeq();
        }

        public virtual DataSegment GetData()
        {
            var data = new DataSegment();
            int length = 0;
            data.ClearPos();
            //data.Write(m_cmd);
            //data.Write(m_ack);
            //data.Write(m_messageID);
            data.Write(m_data,m_length);
            return data;
        }

        public static bool TryGetMessage(DataSegment data,out MessageBase msg)
        {
            var off = data.Pos;

            if (data.TryReadDatas(out var arr, out var length))
            {
                CommonMessage m = new CommonMessage();
                m.MergeFrom(new CodedInputStream(arr));
                var array = m.Body.Value.ToByteArray();
                DebugTools.DebugHelper.Log(m.ToString());
                //DebugHelper.Log($"MessageCommand.ProtoBuf_Message m_readPos :{data.m_readPos}, off = {off}");
                msg = new MessageBase()
                {
                    m_length = array.Length,
                    m_data = array,
                    m_messageID = (int)m.MsgType
                };
                return true;
            }
            

            msg = null;
            data.Length = off;
            return false;
        }

        public void ClearData()
        {
            m_data = null;
        }
    }
}