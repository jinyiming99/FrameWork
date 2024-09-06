using System;
using UnityEngine;
using UnityEngine.Pool;

public class UserBodyPool : IDisposable
{
    private const string TemplatePath = "Body";
    private Transform _root;
    private GameObject _template;
    private IObjectPool<GameObject> _pool;

    public UserBodyPool()
    {
        _root = new GameObject("UserBodyPool").transform;
        UnityEngine.Object.DontDestroyOnLoad(_root.gameObject);
        _template = Resources.Load<GameObject>(TemplatePath);
        _pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 6);
    }

    public GameObject Get()
    {
        return _pool.Get();
    }

    public void Release(GameObject obj)
    {
        _pool.Release(obj);
    }

    private GameObject CreatePooledItem()
    {
        var obj = UnityEngine.Object.Instantiate(_template.gameObject);
        obj.transform.SetParent(_root);
        obj.transform.localScale = Vector3.one;
        return obj;
    }

    private void OnReturnedToPool(GameObject obj)
    {
        obj.SetActive(false);
    }

    private void OnTakeFromPool(GameObject obj)
    {
        obj.SetActive(true);
    }

    void OnDestroyPoolObject(GameObject obj)
    {
        if (obj != null && obj.gameObject != null)
        {
            UnityEngine.Object.Destroy(obj.gameObject);
        }
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(_root.gameObject);
        _template = null;
        _pool.Clear();
        _pool = null;
    }
}