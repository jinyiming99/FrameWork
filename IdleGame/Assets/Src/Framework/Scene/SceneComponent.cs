using System;
using System.Collections;
using System.Collections.Generic;
using GameFrameWork.DebugTools;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameFrameWork
{

    public class SceneComponent : MonoBehaviour
    {
        private int _curSceneIndex = -1;
        private int _loadingSceneIndex = -1;

        // Start is called before the first frame update
        void Start()
        {
            var count = SceneManager.sceneCountInBuildSettings;
            DebugHelper.Log($"Scene count = {count}");
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// 添加场景
        /// </summary>
        /// <param name="sceneIndex"></param>
        private void TransitionAddScene(int sceneIndex, Action<bool> complete)
        {
            if (_loadingSceneIndex != -1)
                return;

            if (_curSceneIndex != -1)
            {
                var handle =
                    SceneManager.UnloadSceneAsync(_curSceneIndex, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
                if (handle is null)
                {
                    DebugHelper.Log($"unload scene {_curSceneIndex} failed ,handle is null");
                }
                handle.completed += (ao) => { LoadScene(sceneIndex, LoadSceneMode.Additive,complete); };
            }
            else
            {
                LoadScene(sceneIndex, LoadSceneMode.Additive,complete);
            }
        }

        /// <summary>
        /// 切换场景
        /// </summary>
        /// <param name="sceneIndex"></param>
        public void TransitionScene(int sceneIndex,Action<bool> complete)
        {
            if (_loadingSceneIndex != -1)
                return;

            LoadScene(sceneIndex, LoadSceneMode.Single,complete);
        }

        private void LoadScene(int sceneIndex, LoadSceneMode mode,Action<bool> complete)
        {
            _loadingSceneIndex = sceneIndex;
            var asyncHandle = SceneManager.LoadSceneAsync(sceneIndex, mode);
            if (asyncHandle == null)
                complete?.Invoke(false);
            asyncHandle.completed += (ao) =>
            {
                _curSceneIndex = sceneIndex;
                _loadingSceneIndex = -1;
                complete?.Invoke(true);
            };
        }

        public void GetCurrentSceneObject()
        {
            var scene = SceneManager.GetSceneAt(_curSceneIndex);
            var root = scene.GetRootGameObjects();
        }
    }
}