using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Tools.AnimationCreate.Editor
{
    public class AnimationCreaterData
    {
        private Dictionary<string, AnimationCurve> dic = new Dictionary<string, AnimationCurve>();
        private AnimationClip clip ;
        public AnimationCreaterData()
        {
            clip = new AnimationClip();
        }

        public void AddPoint(Vector3 position, float time)
        {
            var curvex = GetCurve("localPosition.x");
            var curvey = GetCurve("localPosition.y");
            var curvez = GetCurve("localPosition.z");

            int length = curvez.length;
            if (length > 2)
            {
                Vector3 lastPos = GetPoint(length - 1);
                Vector3 spos = GetPoint(length - 2);
                Vector3 dir = (lastPos - spos).normalized;
                Vector3 dir2 = (position - lastPos).normalized;
                if (Vector3.Angle(dir,dir2) < 0.5f)
                {
                    curvex.RemoveKey(curvex.length - 1);
                    curvey.RemoveKey(curvey.length - 1);
                    curvez.RemoveKey(curvez.length - 1);
                }
            }
            curvex.AddKey( new Keyframe(time, position.x));
            curvey.AddKey( new Keyframe(time, position.y));
            curvez.AddKey( new Keyframe(time, position.z));
        }

        private Vector3 GetPoint(int index)
        {
            var curvex = GetCurve("localPosition.x");
            var curvey = GetCurve("localPosition.y");
            var curvez = GetCurve("localPosition.z");
            
            return new Vector3(curvex.keys[index].value, curvey.keys[index].value, curvez.keys[index].value);
        }

 

        public void AddRotation(Quaternion rotation, float time)
        {
            var curvex = GetCurve("localRotation.x");
            var curvey = GetCurve("localRotation.y");
            var curvez = GetCurve("localRotation.z");
            var curvew = GetCurve("localRotation.w");
            int length = curvez.length;
            if (length > 2)
            {
                Quaternion lasQuaternion = GetQuaternion(length - 1);
                Quaternion sQuaternion = GetQuaternion(length - 2);
                var q1 = Quaternion.Inverse(sQuaternion) * lasQuaternion;
                var q2 = Quaternion.Inverse(lasQuaternion) * rotation;

                if ((q1.eulerAngles - q2.eulerAngles).magnitude <  1f)
                {
                    curvex.RemoveKey(curvex.length - 1);
                    curvey.RemoveKey(curvey.length - 1);
                    curvez.RemoveKey(curvez.length - 1);
                    curvew.RemoveKey(curvew.length - 1);
                }
            }

            var k = new Keyframe(time, rotation.x);
            AddKey(curvex, new Keyframe(time, rotation.x));
            AddKey(curvey, new Keyframe(time, rotation.y));
            AddKey(curvez, new Keyframe(time, rotation.z));
            AddKey(curvew, new Keyframe(time, rotation.w));
        }
        
        private Quaternion GetQuaternion(int index)
        {
            var curvex = GetCurve("localRotation.x");
            var curvey = GetCurve("localRotation.y");
            var curvez = GetCurve("localRotation.z");
            var curvew = GetCurve("localRotation.w");
            
            return new Quaternion(curvex.keys[index].value, curvey.keys[index].value, curvez.keys[index].value, curvew.keys[index].value);
        }

        public void Save(string path,bool saveRotation = true)
        {
            var curvePosX = GetCurve("localPosition.x");
            SetCurveMode(curvePosX);
            clip.SetCurve("", typeof(Transform), "localPosition.x", SetCurveMode(GetCurve("localPosition.x")));
            clip.SetCurve("", typeof(Transform), "localPosition.y", SetCurveMode(GetCurve("localPosition.y")));
            clip.SetCurve("", typeof(Transform), "localPosition.z", SetCurveMode(GetCurve("localPosition.z")));

            var curvex = SetCurveMode(GetCurve("localRotation.x"));
            var curvey = SetCurveMode(GetCurve("localRotation.y"));
            var curvez = SetCurveMode(GetCurve("localRotation.z"));
            var curvew = SetCurveMode(GetCurve("localRotation.w"));
            if (saveRotation)
            {
                clip.SetCurve("", typeof(Transform), "localRotation.x", curvex);
                clip.SetCurve("", typeof(Transform), "localRotation.y", curvey);
                clip.SetCurve("", typeof(Transform), "localRotation.z", curvez);
                clip.SetCurve("", typeof(Transform), "localRotation.w", curvew);
            }
            
            clip.EnsureQuaternionContinuity();
            AssetDatabase.CreateAsset(clip, "Assets" + path);
        }

        private AnimationCurve SetCurveMode(AnimationCurve curve)
        {
            List<Keyframe> list = curve.keys.ToList();
            for (int i = 0; i < list.Count - 1; i++)
            {
                var x1 = list[i].value;
                var x2 = list[i + 1].value;

                var y1 = list[i].time;
                var y2 = list[i + 1].time;

                var k = (x2 - x1) / (y2 - y1);
                var c1 = curve[i];
                var c2 = curve[i + 1];
                
                c1.outTangent = k;
                c2.inTangent = 1 - k;   
                list[i]= c1;
                list[i + 1]= c2;
               
            }
            curve.ClearKeys();
            for (int i = 0; i < list.Count; i++)
            {
                curve.AddKey(list[i]);
            }            
            for (int i = 0; i < list.Count; i++)
            {
                AnimationUtility.SetKeyLeftTangentMode(curve, i, AnimationUtility.TangentMode.Auto);
            }

            return curve;
        }
        private void AddKey(AnimationCurve curve, Keyframe key)
        {
            // if (curve.length > 1)
            // {
            //     //var p1 = GetKeyFrameGrow(curve[curve.length - 2], curve[curve.length - 1], key.value, key.time);
            //     //if (p1 < 1.01f && p1 > 0.99f)
            //     curve.RemoveKey(curve.length-1);
            // }
            curve.AddKey(key);
        }

        private float GetKeyFrameGrow(Keyframe key, Keyframe lastkey, float f, float t)
        {
            var framelast = lastkey;
            var frame = key;
            var p1 = (framelast.value - frame.value) / (framelast.time - frame.time);
            var p2 = (f - framelast.value) / (t - framelast.time);
            var p = p1 / p2;
            return p;
        }
        private AnimationCurve GetCurve(string key)
        {
            if (!dic.TryGetValue(key, out var curve))
            {
                curve = new AnimationCurve();
                dic.Add(key,curve);
            }

            return curve;
        }
    }
}