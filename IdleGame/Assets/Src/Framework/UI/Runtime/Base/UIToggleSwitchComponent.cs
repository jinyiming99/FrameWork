using NaughtyAttributes;
using UnityEngine;

namespace GameFrameWork.UI
{
    public class UIToggleSwitchComponent : ICustomToggleStateChange
    {
        [ReorderableList]
        public GameObject[] _onShowGameObjects; 
        [ReorderableList]
        public GameObject[] _offShowGameObjects;

        public void OnStateChange(bool isOn)
        {
            foreach (var obj in _onShowGameObjects)
                obj.SetActive(isOn);
            foreach (var obj in _offShowGameObjects)
                obj.SetActive(!isOn);
        }
    }
}