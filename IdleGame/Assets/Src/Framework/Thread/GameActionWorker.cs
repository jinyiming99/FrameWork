using System;
using UnityEngine;

namespace GameFrameWork.Thread
{
    public class GameActionWorker : MonoBehaviour
    {
        private ThreadWorker _threadWorker = new ThreadWorker();
        private MainThreadActionWorker _mainThreadActionWorker;

        void Awake()
        {
            _threadWorker = new ThreadWorker();
            _threadWorker.Start();
            _mainThreadActionWorker = gameObject.AddComponent<MainThreadActionWorker>();
        }

        public void OnDestroy()
        {
            _threadWorker.Stop();
            
        }

        public void ThreadPost(Action action)
        {
            _threadWorker.Post(action);
        }

        public void MainPost(Action action)
        {
            _mainThreadActionWorker.Post(action);
        }
    }
}