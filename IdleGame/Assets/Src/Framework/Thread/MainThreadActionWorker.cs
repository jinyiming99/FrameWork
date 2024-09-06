using System;
using GameFrameWork.Containers;
using UnityEngine;

namespace GameFrameWork.Thread
{
    public class MainThreadActionWorker : MonoBehaviour
    {
        private SwapList<Action> m_workList = new SwapList<Action>();
        private object _lock = new object();
        public void Post(Action action)
        {
            lock (_lock)
            {
                m_workList.Add(action);
            }
        }

        private void Update()
        {
            if (m_workList.GetWorkingLength() > 0)
            {
                lock (_lock)
                {
                    m_workList.Swap();
                }

                var list = m_workList.GetWaitingData();
                foreach (var action in list)
                {
                    try
                    {
                        action?.Invoke();
                    }
                    catch (Exception e)
                    {
                        DebugTools.DebugHelper.LogError($"MainThreadActionWorker update error = {e.ToString()}");
                    }
                }
                m_workList.ClearWaitingData();
            }
        }

        private void OnDestroy()
        {
            m_workList.Clear();
        }
    }
}