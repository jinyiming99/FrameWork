using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public class Edges
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
                return point1;
        }

        public Vector3 GetDirection()
        {
            return (point2 - point1).normalized;
        }
        
        public Vector3 GetOtherDirection()
        {
            return (point1 - point2).normalized;
        }
    }
    [System.Serializable]
    public class Line
    {
        public List<Vector2> points = new List<Vector2>() ;
    }
}