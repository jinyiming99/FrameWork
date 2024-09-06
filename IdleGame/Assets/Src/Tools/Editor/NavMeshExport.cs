

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameFrameWork.DebugTools;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.AI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

namespace Src.Editor
{
    public struct Edges
    {
        public Vector3 point1;
        public Vector3 point2;
        public string GetKey()
        {
            return $"{point1}-{point2}";
        }

        public string GetOtherKey()
        {
            return $"{point2}-{point1}";
        }

        public Vector3 GetOtherPosition(Vector3 point)
        {
            if (point == point1)
                return point2;
            else
            {
                return point1;
            }
        }
    }
    [System.Serializable]
    public class Line
    {
        public List<Vector2> points = new List<Vector2>() ;
    }
    [System.Serializable]
    public class ExportLine
    {
        public List<Line> lines = new List<Line>() ;
    }
    public class NavMeshExport
    {
        static Dictionary<string, Edges> dic = new Dictionary<string, Edges>();
        [MenuItem("GameTools/NavMesh Export")]
        public static void ExportNavMesh()
        {
            dic.Clear();
            var navmesh = NavMesh.CalculateTriangulation();
            Debug.Log($"navmesh.indices.Length: {navmesh.indices.Length}, navmesh.vertices.Length: {navmesh.vertices.Length}");

            // GameObject go = new GameObject("mesh");
            // var filter = go.AddComponent<MeshFilter>();
            // var render = go.AddComponent<Renderer>();
            // filter.mesh = new Mesh();
            // filter.mesh.vertices = navmesh.vertices;
            // filter.mesh.indexFormat = IndexFormat.UInt16;
            // filter.mesh.triangles = navmesh.indices;
            ///创建三角形的边
            for (int i = 0; i < navmesh.indices.Length; i += 3)
            {
                AddEdge(new Edges()
                {
                    point1 = navmesh.vertices[navmesh.indices[i]],
                    point2 = navmesh.vertices[navmesh.indices[i+1]]
                });
                AddEdge(new Edges()
                {
                    point1 = navmesh.vertices[navmesh.indices[i+1]],
                    point2 = navmesh.vertices[navmesh.indices[i+2]]
                });
                AddEdge(new Edges()
                {
                    point1 = navmesh.vertices[navmesh.indices[i+2]],
                    point2 = navmesh.vertices[navmesh.indices[i]]
                });
            }
            
            Debug.Log($"dic count = {dic.Count}");
            foreach (var d in dic)
            {
                Debug.Log(d.Key);
            }

            ///将每个点对应的边加入到字典中
            Dictionary<Vector3, List<Edges>> edgesDic = new Dictionary<Vector3, List<Edges>>();
            
            foreach (var d in dic)
            {
                if (!edgesDic.TryGetValue(d.Value.point1,out List<Edges> list))
                {
                    list = new List<Edges>();
                    edgesDic.Add(d.Value.point1, list);
                }
                list.Add(d.Value);
                
                if (!edgesDic.TryGetValue(d.Value.point2 ,out list))
                {
                    list = new List<Edges>();
                    edgesDic.Add(d.Value.point2, list);
                }
                list.Add(d.Value);
            }
            
            var dic1 = edgesDic.Where(node => node.Value.Count == 2).ToDictionary(value=>value.Key,value=>value.Value);
            if (edgesDic.Count != dic1.Count)
            {
                DebugHelper.LogError($"数量不一致");
            }
            
            edgesDic = dic1;

            List<List<Vector3>> listResult = new List<List<Vector3>>();
            for (int i = 0; i < 10; i++)
            {
                if (edgesDic.Count == 0)
                    break;
                
                var list = GetList(edgesDic);
                if (list == null)
                    break;
                listResult.Add(list);
            }

            Export(listResult);
            dic.Clear();
        }

        private static void Export(List<List<Vector3>> listResult)
        {
            ExportLine exportLine = new ExportLine();
            //exportLine.lines = new Line[listResult.Count];
            int i = 0;
            foreach (var list in listResult)
            {
                Line line = new Line();
                line.points = new List<Vector2>();
                //line.points = new Vector2[list.Count];
                for (int index = 0; index < list.Count; index++)
                {
                    var pos = list[index];
                    line.points.Add(new Vector2(pos.x, pos.z));
                }
                exportLine.lines.Add(line);
            }
            string json = EditorJsonUtility.ToJson(exportLine);
            var path = Directory.GetCurrentDirectory();
            var exportPath = Path.Combine(path, "NavMeshExport.json");
            Debug.Log(exportPath);
            using(FileStream file = new FileStream(exportPath, FileMode.OpenOrCreate))
              {
                  using (StreamWriter writer = new StreamWriter(file))
                  {
                      writer.Write(json);
                      Debug.Log("导出成功");
                  }
              }
        }

        /// <summary>
        /// 通过顶点筛选出一条边
        /// </summary>
        /// <param name="edgesDic"></param>
        /// <returns></returns>
        private static List<Vector3> GetList(Dictionary<Vector3, List<Edges>> edgesDic)
        {
            ///选中edgesdic中一个点，然后将这个点的边加入到一个list中，然后从这个点开始遍历，找到下一个点，然后将这个点的边加入到list中，然后继续遍历，直到闭合或者没有下一个点
            if (edgesDic.Keys.Count > 0)
            {
                var arr = edgesDic.Keys.ToArray();
                var start = arr[0];
                var list = new List<Vector3>();
                var node = edgesDic[start];
                if (node.Count != 2)
                {
                    edgesDic.Remove(start);
                    return null;
                }
                var otherPosition1 = node[0].GetOtherPosition(start);
                var otherPosition2 = node[1].GetOtherPosition(start);
                //list.Add(otherPosition1);
                list.Add(start);
                list.Add(otherPosition2);
                edgesDic.Remove(start);
                var current = otherPosition2;
                while (true)
                {
                    if (edgesDic.TryGetValue(current, out List<Edges> list2))
                    {
                        bool isFind = false;
                        for (int i = 0; i < list2.Count; i++)
                        {
                            var p = list2[i].GetOtherPosition(current);
                            if (!list.Contains(p))
                            {
                                isFind = true;
                                list.Add(p);
                                edgesDic.Remove(current);
                                current = p;
                                break;
                            }
                            else
                            {
                                if (p == list[0])
                                {
                                    DebugHelper.Log($"找到{list[0]} ， 结束{current}");
                                }
                            }
                        }

                        if (!isFind)
                        {
                            edgesDic.Remove(current);
                            
                            return list;
                        }
                    }
                    else
                    {
                        return list;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 添加边
        /// </summary>
        /// <param name="e"></param>
        private static void AddEdge(Edges e)
        {
            var key = e.GetKey();
            var otherKey = e.GetOtherKey();
            if (dic.ContainsKey(key) )
            {
                dic.Remove(key);
            }
            else if (dic.ContainsKey(otherKey))
            {
                dic.Remove(otherKey);
            }
            else
            {
                dic.Add(key, e);
                
            }
            
        }
    }
}