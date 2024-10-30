using System;
using UnityEngine;

namespace GameFrameWork.UI
{
    public class UIComponentBase : MonoBehaviour
    {
        protected virtual void Awake()
        {}

        protected virtual void OnEnable()
        {}

        protected virtual void Start()
        {}

        protected virtual void OnDisable()
        {}

        protected virtual void OnDestroy()
        {}
    }
}