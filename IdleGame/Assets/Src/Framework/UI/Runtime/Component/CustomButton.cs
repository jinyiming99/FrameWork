using System;
using GameFrameWork.UI.Enum;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameFrameWork.UI
{
    [RequireComponent(typeof(CanvasRenderer))]
    [AddComponentMenu("UI/Custom/Component/CustomButton", 1200)]
    public class CustomButton : PressComponentBase ,IAutoCreateComponentInterface
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

        public CustomComponentEnum ComponentType { get=> CustomComponentEnum.Button; }
        // public void OnBeginDrag(PointerEventData eventData)
        // {
        //     _isPressing = false;
        //     if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == gameObject)
        //         EventSystem.current.SetSelectedGameObject(null,eventData);
        //     Debug.Log($"OnBeginDrag {gameObject.name}");
        // }
        //
        // public void OnDrag(PointerEventData eventData)
        // {
        //     
        // }
        //
        // public void OnEndDrag(PointerEventData eventData)
        // {
        //     
        // }
    }
}