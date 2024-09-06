using System;
using UnityEngine;

namespace GameFrameWork
{
    
    public class LifecycleComponent
    {
        private Action m_start;
        private Action m_update;
        private Action m_destroy;
        private Action m_willDestroy;
        private Action m_gameOver;
        private Action m_gamePause;
        private Action m_gameResume;

        public Action StartAction
        {
            get { return m_start;}
            set { m_start = value; }
        }

        public Action UpdateAction
        {
            get { return m_update;}
            set { m_update = value; }
        }

        public Action WillDestroyAction
        {
            get { return m_willDestroy;}
            set { m_willDestroy = value; }
        }
        public Action DestroyAction
        {
            get { return m_destroy;}
            set { m_destroy = value; }
        }
        public Action GameOverAction
        {
            get { return m_gameOver;}
            set { m_gameOver = value; }
        }
        public Action GamePauseAction
        {
            get { return m_gamePause;}
            set { m_gamePause = value; }
        }
        public Action GameResumeAction
        {
            get { return m_gameResume;}
            set { m_gameResume = value; }
        }

        public void Start()
        {
            m_start?.Invoke();
        }

        public void Update()
        {
            m_update?.Invoke();
        }

        public void OnDestroy()
        {
            m_destroy?.Invoke();
        }

        
    }
}