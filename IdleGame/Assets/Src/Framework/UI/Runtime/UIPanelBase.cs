using GameFrameWork.Entity;
using Unity.VisualScripting;
using UnityEngine;

namespace GameFrameWork.UI
{
    public class UIPanelBase<T> : GameEntity,IUIPanelBase where T : MonoBehaviour 
    {
        protected T _view;
        public T View => _view;

        protected virtual string UIViewName { get; }
        
        public virtual int GetLayerID { get; }

        protected override void Awake()
        {
            base.Awake();
            LoadResource();
        }

        protected void LoadResource()
        {
            var obj = Resources.Load(UIViewName);
            var go = GameObject.Instantiate(obj);
            _view = go.AddComponent<T>();
            _view.transform.SetParent(transform, false);
            _view.transform.localPosition = Vector3.zero;
            _view.transform.localScale = Vector3.one;
            Show();
        }
        
        public virtual void Show(params object[] args)
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        public virtual void Release()
        {
            GameObject.Destroy(_view);
            Resources.UnloadAsset(_view);
            _view = null;
        }
    }
}