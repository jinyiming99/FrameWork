using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameFrameWork.UI.Editor
{
    public static class UIEditorConfigDefine
    {
        internal static readonly Type[] _uiTypes = new Type[]
        {
            typeof(Button),
            typeof(Image),
            typeof(RectTransform),
            typeof(Text),
            typeof(InputField),
            typeof(Toggle),
            typeof(Slider),
            typeof(Scrollbar),
            typeof(RawImage),
            typeof(TMPro.TextMeshPro),
            typeof(CustomButton),
            typeof(CustomToggle),
            typeof(CustomToggleGroup),
            typeof(CustomLoopView),
        };

        internal static readonly string IgnoreName = "ignore";
    }
}