using Game.UI;
using GameFrameWork;

namespace Game.Status
{
    public class GameMenuStatus : IState
    {
         
        public bool CanEnter(params object[] args)
        {
            return true;
        }

        public void Enter(params object[] args)
        {
            FrameWork.Instance.UIComponent.ShowUI<UIMainMenuPanel>();
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