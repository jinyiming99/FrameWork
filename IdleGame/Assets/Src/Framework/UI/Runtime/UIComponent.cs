using System;
using System.Collections.Generic;
using Game;
using GameFrameWork.Containers;
using GameFrameWork.Entity;
using UnityEngine;

namespace GameFrameWork.UI
{
    public class UIComponent : MonoBehaviour
    {
        public GameObject uiRoot;

        private Transform _layer1;
        private Transform _layer2;
        private Transform _layer3;
        private Transform _layer4;
        private Transform _layer5;
        

        private Dictionary<Type, IUIPanelBase> _dictionary = new Dictionary<Type, IUIPanelBase>();
        private LinkList_LowGC<IUIPanelBase> _list = new LinkList_LowGC<IUIPanelBase>();
        private void Awake()
        {
            uiRoot = GameObject.Find("Canvas");
            _layer1 = uiRoot.transform.Find("Layer1");
            _layer2 = uiRoot.transform.Find("Layer2");
            _layer3 = uiRoot.transform.Find("Layer3");
            _layer4 = uiRoot.transform.Find("Layer4");
            _layer5 = uiRoot.transform.Find("Layer5");
        }

        public T ShowUI<T>(params object[] args) where T : GameEntity,IUIPanelBase
        {
            var type = typeof(T);
            if (!_dictionary.TryGetValue(type, out var ui))
            {
                GameObject obj = new GameObject(type.Name);
                ui = obj.AddComponent<T>();
                
                var parent = GetLayer(ui.GetLayerID);
                obj.transform.SetParent(parent.transform, false);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localScale = Vector3.one;
                _dictionary.Add(type, ui);
                
            }
            ui.Show(args);
            return ui as T;
        }

        public void HideUI<T>(IUIPanelBase iuiPanelBase) where T : GameEntity, IUIPanelBase
        {
            iuiPanelBase.Hide();
            PushUI(iuiPanelBase);
        }

        public void ReleaseUI<T>(IUIPanelBase iuiPanelBase) where T : GameEntity, IUIPanelBase
        {
            iuiPanelBase.Release();
            var type = typeof(T);
            if (_dictionary.ContainsKey(type))
            {
                _dictionary.Remove(type);
            }
            
            
        }

        private Transform GetLayer(int layer)
        {
            switch (layer)
            {
                case 1 : return _layer1;
                case 2 : return _layer2;
                case 3 : return _layer3;
                case 4 : return _layer4;
                case 5 : return _layer5;
            }

            return null;
        }

        private void PushUI(IUIPanelBase iuiPanel)
        {
            _list.Remove(iuiPanel);
            _list.AddLast(iuiPanel);
        }
    }
}