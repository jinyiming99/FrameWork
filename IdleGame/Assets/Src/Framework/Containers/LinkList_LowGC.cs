
using System.Collections;
using System.Collections.Generic;

public class LinkList_LowGC<T> :IEnumerable<T>
{
    LinkedList<T> _data = new LinkedList<T>();
    Stack<LinkedListNode<T>> _cache = new Stack<LinkedListNode<T>>();
    public int Count => _data.Count;
    public LinkedListNode<T> First => _data.First;
    public LinkedListNode<T> Last => _data.Last;
    
    private int _maxCount = 0;
    public int MaxCount
    {
        get => _maxCount;
        set
        {
            _maxCount = value;
            while (_cache.Count > _maxCount)
            {
                _cache.Pop();
            }
        }
    }
    public LinkList_LowGC(int maxCount = 8)
    {
        _maxCount = maxCount;
    }
    
    public void AddLast(T value)
    {
        if (Contains(value))
            return;
        LinkedListNode<T> node = GetNode(value);
        _data.AddLast(node);
    }
    
    public void AddFirst(T value)
    {
        if (Contains(value))
            return;
        LinkedListNode<T> node = GetNode(value);

        _data.AddFirst(node);
    }

    private LinkedListNode<T> GetNode(T value)
    {
        LinkedListNode<T> node;
        if (_cache.Count > 0)
        {
            node = _cache.Pop();
            node.Value = value;
        }
        else
        {
            node = new LinkedListNode<T>(value);
        }

        return node;
    }
    
    public bool Contains(T value)
    {
        return _data.Contains(value);
    }
    
    public void RemoveLast()
    {
        var node = _data.Last;
        _data.RemoveLast();
        if (_cache.Count < _maxCount)
            _cache.Push(node);
    }
    
    public void RemoveFirst()
    {
        var node = _data.First;
        _data.RemoveFirst();
        if (_cache.Count < _maxCount)
            _cache.Push(node);
    }
    
    public void Remove(T value)
    {
        var node = _data.Find(value);
        _data.Remove(node);
        if (_cache.Count < _maxCount)
            _cache.Push(node);
    }
    
    public LinkedListNode<T> Remove(LinkedListNode<T> value)
    {
        _data.Remove(value);
        if (_cache.Count < _maxCount)
            _cache.Push(value);
        return value.Next;
    }
    
    public void Clear()
    {
        _data.Clear();
        _cache.Clear();
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _data.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
