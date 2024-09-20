using System;
using System.Collections.Generic;
using GameFrameWork.Pool;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Profiling;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GameFrameWork.UI
{
    [AddComponentMenu("UI/Custom/CustomLoopView", 100)]
    [System.Serializable]
    [RequireComponent(typeof(ScrollRect))]
    public class CustomLoopView : DropComponentBase
    {
        private class ComponentData
        {
            public GameObject gameObject;
            public PressComponentBase _PressComponentBase;
            public CustomToggle _CustomToggle;
            public CustomButton _CustomButton;
            public ICustomLoopComponent _CustomLoopComponent;
        }
        private class NodeData
        {
            public int Index;
            public Vector2 Pos;
            public bool IsUsed;
            public bool IsOn;
            public ComponentData Item;
            //public 
            public Action<GameObject> action;

            public void SaveData()
            {
                IsUsed = false;
                if (Item._CustomToggle != null)
                    IsOn = Item._CustomToggle.IsOn;
                Item = null;
            }

            public void LoadData(ComponentData item)
            {
                IsUsed = true;
                Item = item;
                if (Item._CustomToggle != null)
                    Item._CustomToggle.IsOn = IsOn;
                Item._CustomLoopComponent?.SetIndex(Index);
                action?.Invoke(Item.gameObject);
            }

            public void Release()
            {
                Item = null;
            }
        }
        
        private ScrollRect _targetScrollRect;
        private RectTransform _content;
        [Header("宽间隔")]
        [SerializeField]
        private float _widthSpacing = 10;
        
        [Header("高间隔")]
        [SerializeField]
        private float _hightSpacing = 10;
        
        [Header("对象宽度")]
        [SerializeField]
        private float _itemWidth = 100;
         
        [Header("对象高度")]
        [SerializeField]
        private float _itemHeight = 100;
        
        [Header("边缘")]
        [SerializeField]
        private Rect _edge = new Rect(0,0,0,0);
        [Header("单列数量")]
        [SerializeField]
        [Min(1)]
        private int _column = 1;
        
        [Header("物品")]
        [SerializeField]
        private GameObject _item;

        private Action<int, int> _action;
        
        private List<NodeData> _nodeDatas = new List<NodeData>();
        
        private PositionContainer _container ;

        private int _startIndex = 0;
        private int _endIndex = 0;
        private void Awake()
        {
            _targetScrollRect = GetComponent<ScrollRect>();
            _targetScrollRect.onValueChanged.AddListener((v) =>
            {
                Refresh();
            });
            _container = new PositionContainer((int)_itemWidth,(int)_itemHeight,(int)_widthSpacing,(int)_hightSpacing,_column);
            if (_content != null)
                _content.pivot = new Vector2(0,1);
            _item.gameObject.SetActive(false);
            _content = _targetScrollRect.content;
            _content.anchorMin = new Vector2(0,1);
            _content.anchorMax = new Vector2(0,1);
            _content.pivot = new Vector2(0,1);
            var toggle = _item.GetComponent<CustomToggle>();
            if (toggle != null)
            {
                var dropComponentBase = _item.AddComponent<DropComponentBase>();
                toggle.SetDropComponent(dropComponentBase);
            }
        }
        

        private void Refresh()
        {
            int startIndex = 0;
            
            if (_targetScrollRect.vertical)
                startIndex = Mathf.FloorToInt((_content.anchoredPosition.y - _edge.yMin) / (_itemHeight + _hightSpacing)) * _column;
            else
                startIndex = Mathf.FloorToInt((-_content.anchoredPosition.x - _edge.xMin) / (_itemWidth + _widthSpacing)) * _column;
            int endIndex = 0;
            RectTransform rectTransform = _targetScrollRect.transform as RectTransform;
            if (_targetScrollRect.vertical)
                endIndex = Mathf.CeilToInt((_content.anchoredPosition.y + rectTransform.sizeDelta.y)/ (_itemHeight + _hightSpacing)) * _column;
            else
                endIndex = Mathf.CeilToInt((-_content.anchoredPosition.x + rectTransform.sizeDelta.x)/ (_itemWidth + _widthSpacing)) * _column;
            if (_startIndex == startIndex && _endIndex == endIndex)
                return;
            for (int i = 0 ; i < _nodeDatas.Count; ++i)
            {
                var nodeData = _nodeDatas[i];
                if (nodeData.IsUsed)
                {
                    if (i < startIndex || i >= endIndex)
                    {
                        ReleaseGameObject(nodeData.Item);
                        nodeData.SaveData();
                    }
                }
            }

            Vector2 v = new Vector2(0, 1);
            for (int i = 0 ; i < _nodeDatas.Count; ++i)
            {
                var nodeData = _nodeDatas[i];
                if (!nodeData.IsUsed)
                {
                    if (i >= startIndex && i < endIndex)
                    {
                        var item = GetGameObject();
                        var rect = item.gameObject.transform as RectTransform;
                        item.gameObject.transform.localPosition = _nodeDatas[i].Pos + (rect.pivot - v) * new Vector2(_itemWidth,-_itemHeight); 
                        item.gameObject.transform.localScale = Vector3.one;
                        item.gameObject.transform.localRotation = Quaternion.identity;
                        nodeData.LoadData(item);
                    }
                }
            }

        }
        
        private ComponentData GetGameObject()
        {
            var go = ClassPool<ComponentData>.Spawn();
            if (go.gameObject == null)
            {
                go.gameObject = GameObject.Instantiate(_item);
                go._PressComponentBase = go.gameObject.GetComponent<PressComponentBase>();
                go._CustomToggle = go.gameObject.GetComponent<CustomToggle>();
                go._CustomButton = go.gameObject.GetComponent<CustomButton>();
                go._CustomLoopComponent = go.gameObject.GetComponent<ICustomLoopComponent>();
                go.gameObject.transform.SetParent(_content);
            }
            go.gameObject.SetActive(true);
            
            return go;
        }
        
        private void ReleaseGameObject(ComponentData go)
        {
            go.gameObject.SetActive(false);
            ClassPool<ComponentData>.Despawn(go);
        }

        public void SetData<T>(List<T> list)
        {
            Profiler.BeginSample("setdata");
            
            _container.Calculate(list.Count,!_targetScrollRect.horizontal);
            var size = new Vector2(_container.ContainerWidth + _edge.xMin + _edge.xMax,_container.ContainerHight+ _edge.yMin + _edge.yMax);
            var rect = _targetScrollRect.transform as RectTransform;
            _content.sizeDelta = new Vector2(Mathf.Max(size.x, rect.sizeDelta.x), Mathf.Max(size.y, rect.sizeDelta.y));

            for (int i = 0; i < list.Count; ++i)
            {
                Profiler.BeginSample("new");
                var node = ClassPool<NodeData>.Spawn();

                node.Index = i;
                node.Pos = _container.GetPos(i) + new Vector2(_edge.xMin, -_edge.yMin);
                node.IsUsed = false;
                node.IsOn = false;
           
                Profiler.EndSample();
                Profiler.BeginSample("action");
                node.action = (go) =>
                {
                    var com = go.GetComponent<ICustomLoopNode<T>>();
                    com.SetData(list[node.Index]);
                };
                Profiler.EndSample();
                _nodeDatas.Add(node);
            }
            

            Profiler.BeginSample("Refresh");
            Refresh();
            Profiler.EndSample();
            Profiler.EndSample();
        }
        
        private void RemoveOff<T>(int count)
        {
            for (int i = 0; i < count; ++i)
            {
                var data = _nodeDatas[_nodeDatas.Count - 1];
                ClassPool<NodeData>.Despawn(data);
                _nodeDatas.RemoveAt(_nodeDatas.Count - 1);
            }
        }

        // private static void SetData<T>(GameObject go)
        // {
        //     var com = go.GetComponent<ICustomLoopNode<T>>();
        //     com.SetData(list[node.Index]);
        // }

        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);
            var pos = GetPointerDragVector2(eventData.position);
            Refresh();
        }
    }
}