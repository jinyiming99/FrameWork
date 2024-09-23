using System;
using UnityEngine;

namespace GameFrameWork.UI
{
    [AddComponentMenu("UI/Custom/Component/CustomButton", 1200)]
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