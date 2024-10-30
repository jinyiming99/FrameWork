using System;
using GameFrameWork.UI.Enum;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

namespace GameFrameWork.UI
{
    [AddComponentMenu("UI/Custom/Component/CustomToggle", 100)]
    public class CustomToggle  : PressComponentBase ,ICustomLoopComponent , IAutoCreateComponentInterface
    {
        public CustomToggleGroup Group;
        [SerializeField]
        protected bool _isOn = false;
        
        ICustomComponentStateChange _stateChange;
        
        private Action<bool> _onValueChanged;
        public Action<bool> OnValueChanged
        {
            get { return _onValueChanged; }
            set { _onValueChanged = value; }
        }

        protected override bool IsSelected => _isOn;

        public bool IsOn
        {
            get { return _isOn; }
            set
            {
                if (_isOn != value)
                {
                    SetToggleState(value);
                    _onValueChanged?.Invoke(_isOn);
                    if (_isOn)
                        Group.SetToggle(this);
                }
            }
        }

        public bool InitIsOn
        {
            set => _isOn = value;
        }
        

        private void SetValue(bool isOn)
        {
            _isOn = isOn;
        }
        
        

        #region MonoBehavior Function

        protected override void Awake()
        {
            base.Awake();
            //RegisterToggle();
            if (Group.IsMuliSelect)
            {
                SetToggleState(_isOn);
            }
        }

        private void OnEnable()
        {
            RegisterToggle();
        }
        
        private void OnDisable()
        {
            UnRegisterToggle();
        }
        
        private void OnDestroy()
        {
            //UnRegisterToggle();
        }

        #endregion
        
        
        private void RegisterToggle()
        {
            if (Group == null)
                return;
            Group.RegisterToggle(this);
        }
        
        private void UnRegisterToggle()
        {
            if (Group == null)
                return;
            Group.UnRegisterToggle(this);
        }

        internal void SetToggleState(bool on)
        {
            _isOn = on;
            SetState(_isOn ? UIComponentStates.selected : _isPointerEnter ? UIComponentStates.hightLight : UIComponentStates.normal);
            SetValue(_isOn);
        }

        protected override void OnClick()
        {
            if (!gameObject.activeInHierarchy)
                return;
            if (Group.IsMuliSelect)
            {
                IsOn = !IsOn;
            }
            else
            {
                if (IsOn)
                    return;
                IsOn = true;
            }
        }

        private int _index = 0;
        public int GetIndex()
        {
            return _index;
        }

        public void SetIndex(int index)
        {
            _index = index;
        }

        public CustomComponentEnum ComponentType { get => CustomComponentEnum.Toggle; }
    }
}