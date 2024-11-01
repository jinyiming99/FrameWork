using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameFrameWork.UI
{
    public class DropComponentBase : UIComponentBase, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        protected bool _isDroging = false;
        public bool IsDroging => _isDroging;
        protected Vector2 _pivot;
        public Vector2 Pivot => _pivot;

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            _isDroging = true;
            _pivot = eventData.position;
            if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == null)
                EventSystem.current.SetSelectedGameObject(this.gameObject, eventData);
        }

        public virtual void OnDrag(PointerEventData eventData)
        {

        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            _isDroging = false;
        }

        protected Vector2 GetPointerDragVector2(Vector2 position)
        {
            Vector2 vector2 = position - _pivot;
            return vector2;
        }
    }
}
