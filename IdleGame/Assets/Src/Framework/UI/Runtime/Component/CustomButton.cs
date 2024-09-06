using System;

namespace GameFrameWork.UI
{
    [System.Serializable]
    public class CustomButton : PressComponentBase
    {
        private Action _action;
        public Action ClickAction
        {
            get { return _action; }
            set { _action = value; }
        }
        protected override void OnClick()
        {
            ClickAction?.Invoke();
        }
    }
}