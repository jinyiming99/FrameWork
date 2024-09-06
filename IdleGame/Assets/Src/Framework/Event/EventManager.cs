using System.Collections.Generic;
using GameFrameWork.Base;
using UnityEngine.EventSystems;
using UnityEngine.Pool;

namespace GameFrameWork
{
    public sealed class EventManager<T> : IReference where T : EventArg
    {
        public delegate void EventHandler(object sender, T arg);

        private Dictionary<int, LinkList_LowGC<EventHandler>> _dictionary = new Dictionary<int, LinkList_LowGC<EventHandler>>();
        public void RegisterEvent(int eventId, EventHandler handler)
        {
            if (!_dictionary.TryGetValue(eventId, out LinkList_LowGC<EventHandler> linkList))
            {
                linkList = new LinkList_LowGC<EventHandler>();
                _dictionary.Add(eventId,linkList);
            }
            linkList.AddLast(handler);
        }

        public void UnregisterEvent(int eventId, EventHandler handler)
        {
            if (_dictionary.TryGetValue(eventId, out LinkList_LowGC<EventHandler> linkList))
            {
                linkList.Remove(handler);
            }
        }
        public void FireEvent(object sender, T arg)
        {
            
        }
        /// <summary>
        /// 线程不安全
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        public void FireEventNow(object sender, T arg)
        {
            if (_dictionary.TryGetValue(arg.GetID(), out LinkList_LowGC<EventHandler> linkList))
            {
                foreach (var handler in linkList)
                {
                    handler(sender, arg);
                }
            }
        }

        public void Release()
        {
            foreach (var linkList in _dictionary.Values)
            {
                linkList.Clear();
            }
            _dictionary.Clear();
        }
    }
}