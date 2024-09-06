using System.Collections.Generic;
using GameFrameWork.DebugTools;
using GameFrameWork.Pool;
using CodedInputStream = Google.Protobuf.CodedInputStream;

namespace GameFrameWork.Network.MessageBase
{
    public abstract class MessageProcessor<T,U> : IMessageProcessor 
                                                    where T : Google.Protobuf.IMessage,new()
                                                    where U : IMessageProcessorCreater,new()
    {
        public static string name = string.Empty;
        public static U GetCreater()
        {
            if (string.IsNullOrEmpty(name))
            {
                name = typeof(T).Name;
            }
            
            return new U();
        }
        protected MessageStruct<T> m_msg;
        public MessageProcessor()
        {
            DebugHelper.Log(()=>$"creater processor {name}");
            m_msg = null;
        }

        public void CreateMessage(MessageBase msgBase)
        {
            if (m_msg == null)
                m_msg = new MessageStruct<T>();
            m_msg.message.MergeFrom(new CodedInputStream(msgBase.m_data));
            //m_msg.ack = msgBase.m_ack;
        }

       
        public abstract void Processor();

        public void ReleaseMessage()
        {

        }
    }
}