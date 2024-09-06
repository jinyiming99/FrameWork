using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropComponentBase : MonoBehaviour , IBeginDragHandler,IDragHandler,IEndDragHandler
{
    protected bool _isDroging = false;
    public bool IsDroging => _isDroging;
    protected Vector2 _pivot;
    public Vector2 Pivot => _pivot;
    [SerializeField]
    private ScrollRect _scrollRect;
    public ScrollRect ScrollRect
    {
        set { _scrollRect = value; }
    }
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        _isDroging = true;
        _pivot = eventData.position;
        if (_scrollRect!= null)
            _scrollRect.OnBeginDrag(eventData);
    }
    
    public virtual void OnDrag(PointerEventData eventData)
    {
        if (_scrollRect!= null)
            _scrollRect.OnDrag(eventData);
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        _isDroging = false;
        if (_scrollRect!= null)
            _scrollRect.OnEndDrag(eventData);
    }
    
    protected Vector2 GetPointerDragVector2(Vector2 position)
    {
        Vector2 vector2 = position - _pivot;
        return vector2;
    }
}
