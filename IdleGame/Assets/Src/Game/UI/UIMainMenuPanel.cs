using System;
using System.Collections.Generic;
using GameFrameWork.UI;
using UnityEngine;

namespace Game.UI
{
    public partial class UIMainMenuPanel : UIBase<UIMainMenuPanelView>
    {
        public override int GetLayerID => 2;
        
        private List<string> _textList = new List<string>()
        {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
        };

        private void Start()
        {
            _view.StartBtn_CustomButton.ClickAction += () => { Debug.Log("Start"); };
        }

        public override void Show(params object[] args)
        {
            base.Show(args);
            _view.TextList_CustomLoopView.SetData<string>(_textList);
        }
    }
}