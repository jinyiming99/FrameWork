using System.Collections.Generic;
using System.IO;
using Export;
using UnityEngine;

namespace Tools
{
    public class CreateMeshStaticFunction
    {
        public static List<Vector3> ConvertPositionList(string path)
        {
            List<Vector3> list = new List<Vector3>();
            using (StreamReader reader = new StreamReader(path))
            {
                var strData = reader.ReadToEnd();
                var strArray = strData.Split("===\n");
                foreach (var str in strArray)
                {
                    if (string.IsNullOrEmpty(str) || str.Equals("\n"))
                        continue;
                    var strArray2 = str.Split('\n');
                    Vector3 vector3 = new Vector3();
                    vector3.x = float.Parse(strArray2[0].Split('=')[1]);
                    vector3.y = float.Parse(strArray2[1].Split('=')[1]);
                    vector3.z = -float.Parse(strArray2[2].Split('=')[1]);
                    if (!list.Contains(vector3))
                        list.Add(vector3);
                    else
                    {
                        list.Add(vector3);
                        break;
                    }
                }
            }

            return list;
        }
        public static List<Vector3> SelectPositionList(List<Vector3> list)
        {
            List<Vector3> outList = new List<Vector3>();
            foreach (var pos in list)
            {
                if (outList.Contains(pos))
                {
                    outList.Add(pos);
                    break;
                }
                else
                {
                    outList.Add(pos);
                }
            }

            return outList;
        }

        public static GameObject CreateWallMeshObject(List<Vector3> list)
        {
            ////判断是否为逆时针
            bool isClockwise = false;
            Vector3 p1 = list[0];
            Vector3 p2 = list[1];
            Vector3 p3 = list[2];
            Vector3 dir1 = p2 - p1;
            Vector3 dir2 = p3 - p2;
            float d = Vector3.Dot(dir1, dir2);
            isClockwise = d > 0;
            var mesh = PointsToMeshCreater.CreateMeshFilter(list,isClockwise);
            GameObject go = new GameObject();
            var meshFilter = go.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;
            
            var render = go.AddComponent<MeshRenderer>();
             Material m = Resources.Load<Material>("AirWalMat");
             render.material = m;
            go.transform.position = Vector3.zero;
            return go;
        }

        public static GameObject CreateTerrainMeshObject(List<Vector3> list)
        {
            ////判断是否为逆时针
            bool isClockwise = false;
            Vector3 p1 = list[0];
            Vector3 p2 = list[1];
            Vector3 p3 = list[2];
            Vector3 dir1 = p2 - p1;
            Vector3 dir2 = p3 - p2;
            float d = Vector3.Dot(dir1, dir2);
            isClockwise = d > 0;
            var mesh = PointsToMeshCreater.CreateTerrainMeshObject(list,isClockwise);
            GameObject go = new GameObject();
            go.name = $"terrain";
            var meshFilter = go.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;
            
            var render = go.AddComponent<MeshRenderer>();
            Material m = Resources.Load<Material>("AirWalMat");
            render.material = m;
            go.transform.position = Vector3.zero;
            return go;
        }
    }
}