using GameFrameWork.Timer;
using UnityEngine;

namespace Framework.Timer
{
    public class TimeComponent : MonoBehaviour
    {
        private TimeTaskManager _timeTaskManager = new TimeTaskManager();
        public TimeTaskManager TimeTaskManager => _timeTaskManager;
        
        private bool _isPause = false;
        public bool IsPause => _isPause;

        private float _timeScale = 1f;

        public float TimeScale
        {
            set
            {
                _timeScale = value;
            }
            get
            {
                return _timeScale;
            }
        } 
        private float _preTime = 1f;
        
        private void Awake()
        {
            _isPause = false;
            _timeScale = 1f;
            _preTime = 1f;
        }

        public void Pause()
        {
            if (IsPause)
                return;
            _isPause = true;
            _preTime = _timeScale;
            _timeScale = 0f;
        }

        public void Resume()
        {
            if (!IsPause)
                return;
            _isPause = false;
            _timeScale = _preTime;
        }

        private void Update()
        {
            if (!IsPause)
                _timeTaskManager.Update(Time.time * _timeScale);
        }

        public float GetScaledTime()
        {
            return Time.time * _timeScale;
        }
    }
}