using System;
using UnityEngine;

namespace GameFrameWork.Entity
{
    public abstract class GameEntity : MonoBehaviour
    {
        protected Transform _transform;
        

        protected virtual void Awake()
        {
            _transform = transform;
        }
    }
}