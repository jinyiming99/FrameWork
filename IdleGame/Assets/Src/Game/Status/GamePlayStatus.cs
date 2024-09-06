using GameFrameWork;

namespace Game.Status
{
    public class GamePlayStatus : IState
    {
        public bool CanEnter(params object[] args)
        {
            return true;
        }

        public void Enter(params object[] args)
        {
            FrameWork.Instance.SceneComponent.TransitionScene(1, (b) =>
            {
                
            });
        }

        public void Update()
        {
            
        }

        public bool CanRelease(params object[] args)
        {
            return true;
        }

        public void Release(params object[] args)
        {
            
        }
    }
}