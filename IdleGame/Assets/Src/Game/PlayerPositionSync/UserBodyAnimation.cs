using UnityEngine;

namespace Src.PlayerPositionSync
{
    public class UserBodyAnimation
    {
        private enum AnimationState
        {
            Walk,
            Squat,
            Idle,
        }

        private const float CrossFadeDuration = 0.5f;

        private Animator _animator;

        private AnimationState _currentAnimationState = AnimationState.Idle;

        public UserBodyAnimation(Animator animator)
        {
            _animator = animator;
        }

        public void Walk()
        {
            PlayAnimation(AnimationState.Walk);
        }

        public void Squat()
        {
            PlayAnimation(AnimationState.Squat);
        }

        public void Idle()
        {
            PlayAnimation(AnimationState.Idle);
        }

        private void PlayAnimation(AnimationState animationState)
        {
            if (_currentAnimationState == animationState)
                return;

            _animator.CrossFade(animationState.ToString(), CrossFadeDuration);
            _currentAnimationState = animationState;
        }
    }
}