using System;
using GameFrameWork.UI;
using UnityEngine;

namespace Game.UI
{
    public partial class UIMainMenuPanel : UIBase<UIMainMenuPanelView>
    {
        public override int GetLayerID => 2;

        private void Start()
        {
            _view.StartBtn_CustomButton.ClickAction += () => { Debug.Log("Start"); };
        }

        public override void Show(params object[] args)
        {
            base.Show();
            
        }
    }
}