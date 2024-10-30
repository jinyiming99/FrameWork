using System;
using System.Collections.Generic;
using System.Linq;
using GameFrameWork.UI.Enum;
using UnityEngine;

namespace GameFrameWork.UI
{
    [AddComponentMenu("UI/Custom/Component/CustomToggleGroup", 100)]
    public sealed class CustomToggleGroup : MonoBehaviour ,IAutoCreateComponentInterface
    {
        private enum ToggleType
        {
            Toggle,
            Select,
        }
        private List<CustomToggle> _toggles = new List<CustomToggle>();

        [SerializeField] private ToggleType _toggleType = ToggleType.Toggle;
        private bool _isNeedUpdate = false;
        public bool IsMuliSelect
        {
            get { return _toggleType == ToggleType.Select; }   
        }
        
        internal void RegisterToggle(CustomToggle toggle)
        {
            if (toggle == null)
                return;
            
            if (!_toggles.Contains(toggle))
            {
                _toggles.Add(toggle);
                _isNeedUpdate = true;
            }
        }
        internal void UnRegisterToggle(CustomToggle toggle)
        {
            if (toggle == null)
                return;
            if (_toggles.Contains(toggle))
            {
                _toggles.Remove(toggle);
                _isNeedUpdate = true;
            }
        }
        
        internal void SetToggle(CustomToggle toggle)
        {

            if (_toggles == null) return;
            foreach (var t in _toggles)
            {
                if (t == toggle)
                    continue;
                t.IsOn = false;
            }
        }

        private void Awake()
        {
            var toggles = GetComponentsInChildren<CustomToggle>();
            _toggles = toggles.Where(toggle =>  toggle.gameObject.activeSelf).ToList();
            CheckToggle();
        }

        private void OnEnable()
        {
            CheckToggle();
        }

        private void Update()
        {
            if (_isNeedUpdate)
            {
                CheckToggle();
                _isNeedUpdate = false;
            }
        }

        private void CheckToggle()
        {
            if (_toggles.Count == 0) return;
            var toggle = ActiveToggles();
            if (toggle == null)
            {
                toggle = _toggles[0];
                _toggles[0].IsOn = true;
                toggle.SetToggleState(true);
            }
            else
            {
                toggle.SetToggleState(true);
            }
            for (int i = 0; i < _toggles.Count; i++)
            {
                if (_toggles[i] == toggle)
                    continue;
                _toggles[i].SetToggleState(false);
            }
        }

        public CustomToggle ActiveToggles()
        {
            for (int i = 0; i < _toggles.Count; i++)
            {
                if (_toggles[i].IsOn)
                    return _toggles[i];
            }
            return null;
        }

        public CustomComponentEnum ComponentType { get => CustomComponentEnum.ToggleGroup; }
    }
}