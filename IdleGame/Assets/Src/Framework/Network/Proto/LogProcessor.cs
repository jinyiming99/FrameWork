using System;
using System.Collections.Generic;
using com.VRDemo.gamemessage;
using GameFrameWork.Network.MessageBase;
using Google.Protobuf.Collections;
using UnityEngine;

namespace Network.Processor
{
    [Processor((int)com.VRDemo.gamemessage.MessageType.EnterScene)]
    public class LoginRes : MessageProcessor<com.VRDemo.gamemessage.LoginReply, MessageProcessorCreater<LoginRes>>
    {
        
        public override void Processor()
        {
            Player.Instance.UserID = m_msg.message.Body.Userid;
            Player.Instance.TeamName = m_msg.message.Body.TeamName;
            List<Vector3> list = new List<Vector3>();
            Debug.Log($"path list count = {m_msg.message.Body.Config.PathList.Count}");
            for (var i = 0; i < m_msg.message.Body.Config.PathList.Count; i++)
            {
                var pos = m_msg.message.Body.Config.PathList[i];
                list.Add(new Vector3(pos.X, 0, pos.Y));
            }

            for (int i = 0; i < list.Count - 1; i++)
            {
                var dis = Vector3.Distance(list[i], list[i + 1]);
                Debug.Log($"dis = {dis}");
            }

        }
    }
}