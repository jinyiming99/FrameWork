using com.VRDemo.gamemessage;
using UnityEngine;

namespace Game.Network
{
    public class ProtoExtention
    {
        /// <summary>
        /// vector3 转成NetVector3
        /// </summary>
        /// <returns></returns>
        public static Position Conversion2NetVector3(UnityEngine.Vector3 vector3)
        {
            Position netVector3 = new Position();
            netVector3.X = vector3.x;
            netVector3.Y = vector3.y;
            netVector3.Z = vector3.z;
            return netVector3;
        }
        
        public static Vector3 Conversion2Vector3(Position netVector3)
        {
            Vector3 vector3 = new Vector3();
            vector3.x = netVector3.X;
            vector3.y = netVector3.Y;
            vector3.z = netVector3.Z;
            return vector3;
        }
    }
}