using System;

namespace GameFrameWork.Network.MessageBase
{
    public class ProcessorAttribute :Attribute
    {
        public int m_id;

        public ProcessorAttribute(int number)
        {
            m_id = number;
        }
    }
}