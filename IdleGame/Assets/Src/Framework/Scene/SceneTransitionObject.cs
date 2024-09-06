using System;
using UnityEngine;

namespace GameFrameWork
{
    public class SceneTransitionObject: MonoBehaviour
    {
        private void Awake()
        {
            GameObject.DontDestroyOnLoad(this.gameObject);
        }

        public void Release()
        {
            DestroyImmediate(gameObject);
        }
    }
}