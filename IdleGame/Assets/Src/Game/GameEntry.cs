using System;
using GameFrameWork;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(FrameWork))]
    [DisallowMultipleComponent]
    public class GameEntry : MonoBehaviour
    {
        private FrameWork _frameWork;
        
        private void Awake()
        {
            _frameWork = GetComponent<FrameWork>();
            _frameWork.Create<IdleGame>();
            _frameWork.Init();
        }

        private void Start()
        {
            
        }
    }
}