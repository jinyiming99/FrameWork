using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Tools.AnimationCreate.Editor
{
    [CustomEditor(typeof(AnimationCreater))]
    public class AnimationCreaterInspector :UnityEditor.Editor
    {
        
        private bool _isStartPoint = false;
        private void Awake()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private void OnDestroy()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (!_isStartPoint && GUILayout.Button("start"))
            {
                _isStartPoint = true;
            }
            if (_isStartPoint && GUILayout.Button("end"))
            {
                _isStartPoint = false;
            }

            if (GUILayout.Button("create animation"))
            {
                var creater = target as AnimationCreater;
                if (creater._curve._pathList == null || creater._curve._pathList.Count < 2)
                    return;

                AnimationCreaterData data = new AnimationCreaterData();
                var startPos = creater._curve._pathList[0];
                data.AddPoint(startPos._position,0f);
                data.AddRotation(Quaternion.LookRotation(startPos._endPoint),0f);
                var dis = creater._speed / creater.frame;
                var frameTime = 1f / creater.frame;
                var distance = creater._curve.GetCurveStepCount(dis,out var distanceList);
                List<float> _speedTime = new List<float>();
                for (int i = 0; i < distanceList.Count; i++)
                {
                    float t = distanceList[i] / creater._speed;
                    _speedTime.Add(t);
                }
                float timeTotal = distance / creater._speed;
                
                float frame = timeTotal / frameTime;
                int frameInt = Mathf.CeilToInt(frame);


                // for (float i = 1; i < frame; i++)
                // {
                //     creater._curve.GetCurvePosByStep(i * frameTime, timeTotal, _speedTime, out var pos,
                //         out var tangent);
                //     data.AddPoint(pos, frameTime * i);
                //     if (creater.isFollow)
                //         data.AddRotation(tangent, frameTime * i); 
                // }
                int disTotal = 0;
                Vector3 temp = creater._curve._pathList[0]._position;
                float dis2 = 0f;
                int frameIndex = 1;
                while(true)
                {
                    var tempDis = dis2;
                    creater._curve.GetCurPosByDis(dis,dis2,temp, distance, distanceList, 
                        out dis2,
                        out var pos,
                        out var tangent);
                    if (tangent.eulerAngles == Vector3.zero)
                    {
                        creater._curve.GetCurPosByDis(dis,tempDis,temp, distance, distanceList, 
                            out dis2,
                            out pos,
                            out tangent);
                    }
                    temp = pos;
                    data.AddPoint(pos, frameTime * frameIndex);
                    if (creater.isFollow)
                        data.AddRotation(tangent, frameTime * frameIndex);
                    //Debug.Log($"{tangent.eulerAngles.ToString()}");
                    frameIndex++;
                    if (dis2 >= distance)
                    {
                        var count = creater._curve._pathList.Count;
                        var pathData = creater._curve._pathList[count - 1];
                        data.AddPoint(pathData._position, timeTotal);
                        if (creater.isFollow)
                            data.AddRotation(Quaternion.LookRotation(pathData._endPoint), timeTotal);
                        break;
                    }
                }
                string path = EditorUtility.SaveFilePanel(string.Empty, "Assets", "animationClip1", "anim");
                path = path.Replace(Application.dataPath, string.Empty);
                if (string.IsNullOrEmpty(path))
                    return;
                Debug.Log(path);
                data.Save(path,creater.isFollow);
                
                // for (int i = 1; i < frame; i++)
                // {
                //     creater._curve.GetCurvePosByStep(i * dis,distance, distanceList, out var pos, out var tangent);
                //     tempPos = pos;
                //     data.AddPoint(pos,frameTime * i);
                //     if (creater.isFollow)
                //         data.AddRotation(tangent,frameTime * i);
                // }
                // string path = EditorUtility.SaveFilePanel(string.Empty, "Assets", "animationClip1", "anim");
                // path = path.Replace(Application.dataPath, string.Empty);
                // if (string.IsNullOrEmpty(path))
                //     return;
                // Debug.Log(path);
                // data.Save(path,creater.isFollow);
            }
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            var creater = target as AnimationCreater;
            ///creater
            if (_isStartPoint && Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                
                Event.current.Use();
                Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    var point = new BezierPoint(hit.point);
                    var size = HandleUtility.GetHandleSize(hit.point) ;
                    point._startPoint = new Vector3(size, 0, 0);
                    point._endPoint = new Vector3(-size, 0, 0);
                    creater._curve._pathList.Add(point);
                    ///creater bezier pos
                    if (creater._curve._pathList.Count > 1) 
                    {
                    
                    }
                }

                
            }

            if (creater._curve._pathList.Count < 2)
                return;
            ///drawline
            for (int i = 0; i < creater._curve._pathList.Count - 1; i++)
            {
                Handles.DrawBezier(creater._curve._pathList[i]._position,
                    creater._curve._pathList[i+1]._position,
                    creater._curve._pathList[i].EndPoint,
                    creater._curve._pathList[i+1].StartPoint,
                    Color.cyan, 
                    null,
                    2f);
            }
            foreach (var pos in creater._curve._pathList)
            { 
                EditorGUI.BeginChangeCheck();
                Vector3 newPosition = Handles.FreeMoveHandle(pos._position, 
                    HandleUtility.GetHandleSize(pos.StartPoint)*0.1f, Vector3.zero, Handles.SphereHandleCap);
                if (EditorGUI.EndChangeCheck())
                    pos._position = newPosition;
                
                EditorGUI.BeginChangeCheck();
                Vector3 newStartPosition = Handles.FreeMoveHandle(pos.StartPoint, 
                    HandleUtility.GetHandleSize(pos.StartPoint)*0.1f, Vector3.zero, Handles.CubeHandleCap);
                if (EditorGUI.EndChangeCheck())
                {
                    pos._startPoint = newStartPosition - pos._position;
                    var dir = pos._startPoint.normalized;
                    var m = pos._endPoint.magnitude;
                    pos._endPoint = -dir * m;
                }
                
                EditorGUI.BeginChangeCheck();
                Vector3 newEndPosition = Handles.FreeMoveHandle(pos.EndPoint, 
                    HandleUtility.GetHandleSize(pos.EndPoint)*0.1f, Vector3.zero, Handles.CubeHandleCap);
                if (EditorGUI.EndChangeCheck())
                {
                    pos._endPoint = newEndPosition - pos._position;
                    var dir = pos._endPoint.normalized;
                    var m = pos._startPoint.magnitude;
                    pos._startPoint = -dir * m;
                }
                Handles.DrawLine(pos._position,pos.StartPoint);
                Handles.DrawLine(pos._position,pos.EndPoint);
            }
            // var dis = creater._speed / creater.frame;
            // var frameTime = 1f / creater.frame;
            // var distance = creater._curve.GetCurveStepCount(dis,out var distanceList);
            // float timeTotal = distance / creater._speed;
            // float frame = timeTotal / frameTime;
            // int frameInt = Mathf.CeilToInt(frame);
            // List<float> _speedTime = new List<float>();
            // for (int i = 0; i < distanceList.Count; i++)
            // {
            //     float t = distanceList[i] / creater._speed;
            //     _speedTime.Add(t);
            // }
            //
            // int disTotal = 0;
            // Vector3 temp = creater._curve._pathList[0]._position;
            // float dis2 = 0f;
            // while (true)
            // {
            //     creater._curve.GetCurPosByDis(dis,dis2,temp, distance, distanceList, 
            //         out dis2,
            //         out var pos,
            //         out var tangent);
            //     temp = pos;
            //     // var f = HandleUtility.GetHandleSize(pos) * 0.1f;
            //     // Handles.DrawWireCube(pos,new Vector2(f,f));
            //     
            //     if (dis2 >= distance)
            //         return;
            // }
        }
        
        //private void 
    }
}