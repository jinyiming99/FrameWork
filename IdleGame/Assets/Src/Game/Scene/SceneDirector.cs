using System;
using UnityEngine;


public class SceneDirector : MonoBehaviour
{
    [SerializeField]
    private Vector3[] _scenePositions;

    [SerializeField] private SceneNode[] _sceneNode;
    [SerializeField] private int _currentSceneIndex = 0;
    //[SerializeField] private NavmeshPathDraw _pathDraw;
    [SerializeField] private GameObject _go;
    
    ///设置场景位置
    public void SetScenePositions(Vector3[] positions)
    {
        _scenePositions = positions;
    }

    /// <summary>
    /// 开始播放场景
    /// </summary>
    public void StartScene()
    {
        if (_sceneNode == null)
            return;
        ShowScene(_currentSceneIndex);
    }

    /// <summary>
    /// 下一场景
    /// </summary>
    private void NextScene()
    {
        var curScene = _sceneNode[_currentSceneIndex];
        curScene.gameObject.SetActive(false);
        _currentSceneIndex++;

        ShowScene(_currentSceneIndex);
    }
    /// <summary>
    /// 显示场景
    /// </summary>
    /// <param name="index"></param>
    private void ShowScene(int index)
    {
        if (_sceneNode.Length <= index)
            return;
        var node = _sceneNode[index];
        node.gameObject.SetActive(true);
        node.RotateSceneNode(_scenePositions[index], _scenePositions[index + 1]);
        //node.SetSceneChangeCallback(NextScene);
        //_pathDraw.DrwLineWithTarget(_scenePositions[index + 1]);
        _go.transform.position = _scenePositions[index+1];
    }

    private void OnDrawGizmos()
    {
        if (_scenePositions == null || _scenePositions.Length == 0)
            return;
        for (int i = 0; i < _scenePositions.Length; i++)
        {
            Gizmos.DrawSphere(_scenePositions[i], 0.5f);
        }
    }
}
