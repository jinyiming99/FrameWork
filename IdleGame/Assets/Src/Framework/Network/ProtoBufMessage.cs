using System;
using System.Reflection;
using GameFrameWork.Network.MessageBase;
using Google.Protobuf;
using UnityEngine;

namespace Game.Network
{
    public class ProtoBufMessageDistribute : MessageDistribute
    {
        private string spaceName = "Network.Processor";
        
        public ProtoBufMessageDistribute(){ } 
        public ProtoBufMessageDistribute(string space = "")
        {
            if (space != "")
                spaceName = space;
        }
        public override void CreateMessageDic()
        {
            Assembly asm = null;
            var types =GameFrameWork.Util.AssemblyTool.FindTypeBase(typeof(IMessageProcessor));
            foreach (System.Type type in types)
            {
                if (type.Namespace == spaceName && !type.IsAbstract)
                { 
                    var att = type.GetCustomAttribute<ProcessorAttribute>(); 
                    MethodInfo info = type.BaseType.GetMethod("GetCreater");
                    if (att != null)
                    {
                        IMessageProcessorCreater builder = info.Invoke(null, null) as IMessageProcessorCreater;
                        m_createrDic[att.m_id] = builder;
                    }
                }
            }
        }
    }
}