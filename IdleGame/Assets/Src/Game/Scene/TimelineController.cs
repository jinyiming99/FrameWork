using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Game.Scene
{
    public class TimelineController : MonoBehaviour
    {
        PlayableDirector timeline;

        private Action _completeCallback = null;

        public Action CompleteCallback
        {
            set => _completeCallback = value;
            get => _completeCallback;
        }

        void Awake()
        {
            timeline = GetComponent<PlayableDirector>();
            timeline.paused += OnPaused;
            timeline.stopped += OnStopped;
            timeline.played += OnPlayed;
        }

        public void Play()
        {
            Debug.Assert(timeline != null,$" timeline is null");
            timeline.initialTime = 0f;
            timeline.Play();
        }

        public void Play(float startTime)
        {
            Debug.Assert(timeline != null,$" timeline is null");
            if (timeline is null)
                return;
            Debug.Assert(startTime >=0f && startTime <= timeline.playableAsset.duration,$" startTime is out of range");

            timeline.initialTime = startTime;
            timeline.Play();
        }

        private void OnPlayed(PlayableDirector director)
        {
            
        }

        private void OnPaused(PlayableDirector director)
        {
            Debug.Log($" Paused");
        }

        private void OnStopped(PlayableDirector director)
        {
            Debug.Log($" stopped time {director.time} , duration {director.playableAsset.duration}");
            if (director.time == 0f && director.playableAsset.duration > 0f)
            {
                Debug.Log("Reached end of timeline");
            }
        }
    }
}