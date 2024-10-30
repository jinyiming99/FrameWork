using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.AnimationCreate
{
    [Serializable]
    public class BezierCurve
    {
        public List<BezierPoint> _pathList = new List<BezierPoint>();
        
        private Vector3 GetCurvePos(Vector3 start, Vector3 end, float f)
        {
            return Vector3.Lerp(start, end, f);
        }

        private Vector3 GetCurvePos2(Vector3 p1, Vector3 p2, Vector3 p3, float f)
        {
            var startpos = GetCurvePos(p1, p2, f);
            var endpos = GetCurvePos(p2, p3, f);
            return GetCurvePos(startpos, endpos, f);
        }

        private Vector3 GetCurvePos3(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float f)
        {
            var s1 = GetCurvePos(p1, p2, f);
            var s2 = GetCurvePos(p2, p3, f);
            var s3 = GetCurvePos(p3, p4, f);

            return GetCurvePos2(s1, s2, s3, f);
        }

        private Vector3 GetCurvePos3(BezierPoint start,BezierPoint end, float f)
        {
            return GetCurvePos3(start._position, start.EndPoint, end.StartPoint, end._position, f);
        }

        private Vector3 GetCurveTangent(BezierPoint start,BezierPoint end, float f)
        {
            var s2 = GetCurvePos(start.EndPoint, end.StartPoint, f);
            var s3 = GetCurvePos(end.StartPoint, end._position, f);
            var endpos = GetCurvePos(s2, s3, f);
            return endpos;
        }

        public bool GetCurPosByDis(float dis, float enterDis, Vector3 ve,float distance, List<float> distanceList, 
            out float outDis,out Vector3 outPos, out Quaternion tangent)
        {
            Vector3 temp = ve;
            float curDis = enterDis + dis;
            outPos = Vector3.zero;
            tangent = Quaternion.identity;
            outDis = curDis;
            int index = 0;
            float off = dis;
            Vector3 tempPos = ve;
            Quaternion temptangent = Quaternion.identity;
            float pper = 10f;
            float tempdis = curDis;
            while (true)
            {
                if (GetCurvePosByStep(curDis, distance, distanceList, out outPos, out tangent))
                {
                    var d = Vector3.Distance(temp, outPos);
                    var per = d / dis;
                    var a = Mathf.Abs(1 - per);

                    if (per > 1.1 || per < 0.9)
                    {
                        if (per > 1.1)
                            off -= off * 0.3f;
                        else
                            off += off * 0.3f;
                        if (pper > a)
                        {
                            pper = a;
                            tempdis = curDis;
                            tempPos = outPos;
                            temptangent = tangent;
                        }
                        curDis = enterDis + off;
                    }
                    else
                    {
                        outDis = curDis;
                        return true;
                    }
                }

                if (index++ > 10)
                {
                    outDis = tempdis;
                    outPos = tempPos;
                    tangent = temptangent;
                    return false;
                }
            }

            return false;
        }

        public bool GetCurvePosByStep(float curStep,float distance,List<float> distanceList, out Vector3 outPos, out Quaternion tangent)
        {
            float total = distance;
            if (total <= curStep)
            {
                outPos = _pathList[^1]._position;
                tangent = Quaternion.LookRotation(_pathList[^1]._endPoint);

                return true;
            }

            float index = 0;
            for (int i = 0; i < _pathList.Count - 1; i++)
            {
                float step = distanceList[i];
                if (curStep - index < step)
                {
                    float f = (curStep - index) / step;
                    var start = _pathList[i];
                    var end = _pathList[i + 1];
                    outPos = GetCurvePos3(start, end, f);
                    var tangentPos = GetCurveTangent(start, end, f);
                    tangent = Quaternion.LookRotation(tangentPos - outPos); 
                    return true;
                }
                else
                {
                    index += step;
                }
            }

            outPos = Vector3.zero;
            tangent = Quaternion.identity;
            return false;
        }
        

        public float GetCurveStepCount(float stepDis,out List<float> stepDistanceList)
        {
            float totalstep = 0f;
            stepDistanceList = new List<float>();
            for (int i = 0; i < _pathList.Count - 1; i++)
            {
                float step = GetDistance(_pathList[i], _pathList[i + 1], stepDis);
                //Debug.Log($"Step = {step}");
                totalstep += step;
                stepDistanceList.Add(step);
            }

            return totalstep;
        }

        private float GetDistance(BezierPoint startPos, BezierPoint endPos,float stepDis)
        {
            float totalDis = 0f;
            float testCount = TestStep(startPos, endPos, stepDis);
            Vector3 temp = startPos._position;
            for (int i = 0; i < testCount; i++)
            {
                float f = (float)i / testCount;
                var pos = GetCurvePos3(startPos, endPos, f);
                var d = Vector3.Distance(temp, pos);
                temp = pos;
                totalDis += d;
            }

            return totalDis;
        }

        private float TestStep(BezierPoint startPos , BezierPoint endPos,float stepDis)
        {
            float step = 1000;
            //确认step次数
            var start = startPos;
            var end = endPos;
            while (true)
            {
                var pos = GetCurvePos3(start._position, start.EndPoint, end.StartPoint, end._position, 1f / step);
                var dis = Vector3.Distance(start._position, pos);
                var p = dis / stepDis;
                if (p < 1f)
                {
                    return step;
                }
                else
                {
                    step *= p;
                }
            }
            
        }
    }
}