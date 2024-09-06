using System;
using System.Runtime.Versioning;
using Framework.Timer;
using GameFrameWork.Network;
using GameFrameWork.Network.Client;
using GameFrameWork.Network.WebSocket;
using GameFrameWork.Thread;
using GameFrameWork.Timer;
using GameFrameWork.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameFrameWork
{
    [RequireComponent(typeof(FrameWorkConfig))]
    [DisallowMultipleComponent]
    public class FrameWork : MonoBehaviour
    {
        public static FrameWork Instance;
        
        private IGame _game;

        private bool _isRelease = false;

        /// <summary>
        /// 
        /// </summary>
        private GameActionWorker _actionWorker;
        public GameActionWorker ActionWorker => _actionWorker;

        private NetworkComponent _networkComponent; 
        public NetworkComponent NetworkComponent => _networkComponent;

        private SceneComponent _sceneComponent;
        public SceneComponent SceneComponent => _sceneComponent;
        
        
        private LifecycleComponent _lifecycleComponent;
        public LifecycleComponent LifecycleComponent => _lifecycleComponent;
        
        private TimeComponent _timeComponent; 
        public TimeComponent TimeComponent => _timeComponent;
        
        private UIComponent _uiComponent;
        public UIComponent UIComponent => _uiComponent;
        
        internal FrameWorkConfig _config;

        public void Create<T>() where T :class,IGame ,new ()
        {
            _game = new T();
        }

        public void Init()
        {
            Instance = this;
            _isRelease = false;
            _config = GetComponent<FrameWorkConfig>();
            _actionWorker = gameObject.AddComponent<GameActionWorker>();
            _sceneComponent = gameObject.AddComponent<SceneComponent>();
            _networkComponent = gameObject.AddComponent<NetworkComponent>();
            _timeComponent = gameObject.AddComponent<TimeComponent>();
            _uiComponent = gameObject.AddComponent<UIComponent>();
            _lifecycleComponent = new LifecycleComponent();
            _game.Init();
        }
        public void Start()
        {
            _game.Start();
            _lifecycleComponent.Start();
        }

        public void Update()
        {
            _game.Update();
            _lifecycleComponent.Update();
        }
        
        private void Resume()
        {
            _game.Resume();
            _timeComponent.Resume();
            _lifecycleComponent.GameResumeAction?.Invoke();
        }
        
        private void Pause()
        {
            _game.Pause();
            _timeComponent.Pause();
            _lifecycleComponent.GameResumeAction?.Invoke();
        }

        public void WillRelease()
        {
            _isRelease = true;
            _game.Release();
            _lifecycleComponent.WillDestroyAction?.Invoke();
        }

        public void Release()
        {
            _lifecycleComponent.OnDestroy();
        }
        
        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
                Resume();
        }

        private void OnApplicationQuit()
        {
            WillRelease();
            Release();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
                Pause();
        }

        public bool IsStopWork()
        {
            if (!Application.isPlaying)
                return true;
            return _isRelease;
        }
    }
}