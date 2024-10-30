using System;
using UnityEngine;

namespace Tools.AnimationCreate
{
    [Serializable]
    public class BezierPoint
    {
        public Vector3 _position;
        public Vector3 _startPoint;
        public Vector3 _endPoint;

        public BezierPoint(Vector3 position)
        {
            this._position = position;
            _startPoint = Vector3.zero;
            _endPoint = Vector3.zero;
        }

        public Vector3 StartPoint => _position + _startPoint;
        public Vector3 EndPoint => _position + _endPoint;
    }
}