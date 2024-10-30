using System.Collections.Generic;
using System.Linq;
using Tools;
using UnityEngine;
using UnityEngine.Rendering;

namespace Export
{
    public static class PointsToMeshCreater
    {
        public static Mesh CreateMeshFilter(List<Vector3> point3Ds,bool isClockwise)
        {
            if (point3Ds.Count == 0)
                return null;
            var mesh = new Mesh();

            List<Vector3> listPos = new List<Vector3>();
            List<int> listIndex = new List<int>();
            float dis = 0f;
            List<Vector2> UVList = new List<Vector2>();
            for (int i = 0; i < point3Ds.Count-1; i++)
            {
                Vector3 p1 = point3Ds[i];
                Vector3 p2 = point3Ds[(i + 1) % point3Ds.Count];
                float f = Vector3.Distance(p1, p2);
                Vector3 p3 = p1 + Vector3.up * 3;
                Vector3 p4 = p2 + Vector3.up * 3;
                
                listPos.Add(p1);
                listPos.Add(p2);
                listPos.Add(p3);
                listPos.Add(p4);
                
                UVList.Add(new Vector2(dis,p1.y));
                UVList.Add(new Vector2(dis+f,p2.y));
                UVList.Add(new Vector2(dis,p3.y));
                UVList.Add(new Vector2(dis+f,p4.y));
                dis += f;
                listIndex.Add(i * 4);
                listIndex.Add(i * 4 + (isClockwise ? 1 : 3));
                listIndex.Add(i * 4 + (isClockwise ? 3 : 1)) ;
                listIndex.Add(i * 4);
                listIndex.Add(i * 4 + (isClockwise ? 3 : 2));
                listIndex.Add(i * 4 + (isClockwise ? 2 : 3));
            }

            mesh.vertices = listPos.ToArray();
            mesh.indexFormat = IndexFormat.UInt16;
            mesh.triangles = listIndex.ToArray();
            mesh.uv = UVList.ToArray();
            mesh.RecalculateNormals();
            return mesh;
        }

        public static Mesh CreateTerrainMeshObject(List<Vector3> point3Ds,bool isClockwise)
        {
            if (point3Ds.Count == 0)
                return null;


            List<Vector3> listPos = new List<Vector3>();
            List<int> listIndex = new List<int>();
            float dis = 0f;
            List<Vector2> UVList = new List<Vector2>();
            var edges = GetEdges(point3Ds);
            var triangles = GetTriangles(point3Ds,edges);
            if (triangles.Count == 0)
                return null;
            var mesh = new Mesh();
            mesh.vertices = point3Ds.ToArray();
            for (int i = 0; i < triangles.Count; i++)
            {
                var triangle = triangles[i];
                var index1 = point3Ds.FindIndex(pos => triangle._Vector3s[0] == pos);
                var index2 = point3Ds.FindIndex(pos => triangle._Vector3s[1] == pos);
                var index3 = point3Ds.FindIndex(pos => triangle._Vector3s[2] == pos);
                listIndex.Add(index1);
                listIndex.Add(index2);
                listIndex.Add(index3);
            }

            mesh.triangles = listIndex.ToArray();
            mesh.indexFormat = IndexFormat.UInt16;
            mesh.RecalculateNormals();
            return mesh;
        }
        
        public class PointAndEdge
        {
            public Vector3 _point;
            public Edges _edge1;
            public Edges _edge2;
            
            public Triangle GetTriangle()
            {
                return new Triangle()
                {
                    _Vector3s = new[]
                    {
                        _point,
                        _edge1.point1 == _point ? _edge1.point2 : _edge1.point1,
                        _edge2.point1 == _point ? _edge2.point2 : _edge2.point1
                    },
                };
            }

            public Edges GetOtherEdges()
            {
                return new Edges()
                {
                    point2 = _edge1.point1 == _point ? _edge1.point2 : _edge1.point1,
                    point1 = _edge2.point1 == _point ? _edge2.point2 : _edge2.point1
                };
            }
        }

        public class Triangle
        {
            public Vector3[] _Vector3s;

            public Vector3 this[int index]
            {
                get { return _Vector3s[index]; }
                set { _Vector3s[index] = value; }
            }
        }

        private static Dictionary<Vector3 ,PointAndEdge> GetEdges(List<Vector3> point3Ds)
        {
            point3Ds.RemoveAt(point3Ds.Count - 1);
            Dictionary<Vector3 ,PointAndEdge> list = new Dictionary<Vector3 ,PointAndEdge>();
            for (int i = 0; i < point3Ds.Count - 1; i++)
            {
                PointAndEdge e = new PointAndEdge();
                e._point = point3Ds[i];
                e._edge1 = new Edges();
                e._edge1.point1 = point3Ds[i];
                e._edge1.point2 = point3Ds[(i + 1) % point3Ds.Count];
                e._edge2 = new Edges();
                e._edge2.point1 = point3Ds[(i - 1 + point3Ds.Count) % point3Ds.Count];
                e._edge2.point2 = point3Ds[i];
                list.Add(point3Ds[i],e);
            }

            return list;
        }

        private static List<Triangle> GetTriangles(List<Vector3> point3Ds,Dictionary<Vector3, PointAndEdge> list)
        {
            List<Triangle> triangles = new List<Triangle>();
            int count = 0;
            while (list.Count > 0)
            {

                foreach (var pe in list)
                {
                    var dir2 = pe.Value._edge2.GetDirection();
                    var dir1 = pe.Value._edge1.GetDirection();
                    count++;
                    if (count > 100)
                        return triangles;
                    if ( Vector3.Cross(dir2, dir1).y >= 0 && IsInTriangle(point3Ds,pe.Value.GetTriangle()))
                    {
                        ///凹多边形
                        var otherEdges = pe.Value.GetOtherEdges();
                        var triangle = pe.Value.GetTriangle();
                        triangles.Add(triangle);
                        list.Remove(pe.Key);
                        ChangeEdges(point3Ds,list, pe.Value._edge1, otherEdges);
                        ChangeEdges(point3Ds,list, pe.Value._edge2, otherEdges);
                        
                        break;
                    }
                    else
                    {
                        ///凸多边形
                    }
                }
            }

            return triangles;
        }

        private static void ChangeEdges(List<Vector3> point3Ds,Dictionary<Vector3, PointAndEdge> list, Edges removeEdges,Edges changeEdges)
        {
            foreach (var edge in list)
            {
                if (edge.Value._edge1.GetKey() == removeEdges.GetKey())
                {
                    var temp = edge.Value._edge1;
                    edge.Value._edge1 = changeEdges;
                    // var tri = edge.Value.GetTriangle();
                    // if (IsInTriangle(point3Ds,tri))
                    // {
                    //     edge.Value._edge1 = temp;
                    // }
                    return;
                }
                if (edge.Value._edge2.GetKey() == removeEdges.GetKey())
                {
                    var temp = edge.Value._edge2;
                    edge.Value._edge2 = changeEdges;
                    // var tri = edge.Value.GetTriangle();
                    // if (IsInTriangle(point3Ds,tri))
                    // {
                    //     edge.Value._edge2 = temp;
                    // }
                    return;
                }
            }
        }

        private static bool IsInTriangle(List<Vector3> points, Triangle triangle)
        {
            for (int index = 0; index < points.Count; index++)
            {
                bool isIn = false;
                int count = 0;
                if (triangle._Vector3s.Contains(points[index]))
                    continue;
                for (int i = 0; i < triangle._Vector3s.Length; i++)
                {

                    Vector3 dir = triangle[(i + 1) % triangle._Vector3s.Length] - triangle[i];
                    Vector3 dir2 = points[index] - triangle[i];
                    if (Vector3.Cross(dir, dir2).y > 0)
                    {
                        count++;
                    }
                }

                if (count == 3)
                    return false;
            }

            return true;
        }
    }
}