using System;
using Game.Scene;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;


public class SceneNode : MonoBehaviour
{
    [SerializeField]
    private Vector3[] points;

    [SerializeField] private TimelineController[] _playableDirector;

    private void Start()
    {
        if (_playableDirector == null || _playableDirector.Length == 0)
            return;
        _playableDirector[0].Play(5f);
    }
    
    /// <summary>
    /// 旋转场景
    /// </summary>
    public void RotateSceneNode(Vector3 p1, Vector3 p2)
    {
        Vector3 moveDir = GetMoveDir();
        ///服务器传过来的两个点
        Vector3 serverDir = p2 - p1;
        float angle = Vector3.Angle(serverDir, moveDir);
        transform.position = p1;
        var v = Vector3.Cross(serverDir, moveDir);
        if (v.y < 0 )
            transform.Rotate(0, angle, 0);
        else
            transform.Rotate(0, -angle, 0);
    }

    private void OnDrawGizmos()
    {
        if (points == null || points.Length == 0)
            return;
        for (int i = 0; i < points.Length; i++)
        {
            Gizmos.DrawSphere(points[i], 0.1f);
            if (i < points.Length - 1)
                Gizmos.DrawLine(points[i], points[i + 1]);
        }
    }
    
    private Vector3 GetMoveDir()
    {
        if (points.Length >= 2)
            return points[1] - points[0];
        return Vector3.zero;
    }
}
