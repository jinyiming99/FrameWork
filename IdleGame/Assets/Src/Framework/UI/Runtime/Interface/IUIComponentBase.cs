using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameFrameWork.UI
{
    public abstract class IUIComponentBase : MonoBehaviour
    {
        protected UIComponentStates _state;
        protected bool _isWorking = true;
        public virtual bool IsWorking
        {
            get { return _isWorking; }
            set { _isWorking = value; }
        }
        protected virtual void OnDisable()
        {
            //SetState(UIComponentStates.disable);
        }

        protected virtual void OnEnable()
        {
            SetState(UIComponentStates.normal);
        }


        public virtual void SetState(UIComponentStates state , PointerEventData eventData = null)
        {
            if (state == _state)
                return;
            _state = state;
        }
    }
}