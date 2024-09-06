using System;
using System.Threading;
using GameFrameWork.Containers;
using UnityEngine;
using UnityEngine.Profiling;

namespace GameFrameWork.Thread
{
    public class ThreadActionWorker : MonoBehaviour
    {
        enum WorkerStatus
        {
            Null,
            Running,
            Release,
            GameQuit,
        }
        static SwapList<Action> m_swaqList = new SwapList<Action>();

        private static System.Threading.Thread s_thread = null;
        private static bool s_isrun = false;
        private static int s_mainThreadID = 0;

        private static ThreadActionWorker s_worker = null;
        private static WorkerStatus s_enum = WorkerStatus.Null;

        private void Awake()
        {
            Create();
        }

        private void OnDestroy()
        {
            Release();
        }

        private void OnApplicationQuit()
        {
            if (s_enum == WorkerStatus.Running)
            {
                s_isrun = false;
                s_thread.Join();
                s_thread = null;
                s_enum = WorkerStatus.GameQuit;
                if (s_worker != null)
                    GameObject.DestroyImmediate(s_worker);
                s_worker = null;
                ///防止在删除时有任务没有执行
                if (m_swaqList.GetWaitingLength() > 0)
                {
                    foreach (var v in m_swaqList.GetWaitingData())
                    {
                        v?.Invoke();
                    }
                }

                m_swaqList.Clear();
            }
            
        }

        private static void CreateObject()
        {
            //Profiler.BeginSample("CreateObject");
            if (s_worker == null && (s_enum == WorkerStatus.Null || s_enum == WorkerStatus.Release))
            {
                GameObject go = new GameObject();
                go.name = "ThreadActionWorker";
                go.hideFlags = HideFlags.HideAndDontSave;
                s_worker = go.AddComponent<ThreadActionWorker>();
                DontDestroyOnLoad(go);
            }
            //Profiler.EndSample();
        }

        private static void Create()
        {
            if (s_enum == WorkerStatus.Null || s_enum == WorkerStatus.Release)
            {
                s_isrun = true;
                s_mainThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
                s_thread = new System.Threading.Thread(Run);
                s_thread.Start();
                s_enum = WorkerStatus.Running;
            }
        }

        private static void Release()
        {
            if (s_enum == WorkerStatus.Running)
            {
                s_isrun = false;
                s_thread.Join();
                s_thread = null;
                if (s_worker != null)
                    GameObject.DestroyImmediate(s_worker);
                s_worker = null;
                m_swaqList.Clear();
            }
            if (s_enum < WorkerStatus.Release)
                s_enum = WorkerStatus.Release;
        }
        public static void Add(Action action)
        {
            if (action == null)
                return;
            //Profiler.BeginSample("Add");
            CreateObject();
            m_swaqList.Add(action);
            //Profiler.EndSample();
        }

        public static bool Remove(Action action)
        {
            return m_swaqList.TryRemove(action);
        }

        static void Run()
        {
            Profiler.BeginThreadProfiling("thread run ","my thread");
            
            while (s_isrun)
            {
                Profiler.BeginSample("thread update");
                int count = m_swaqList.GetWorkingLength();
                //Debug.Log($"m_swaqList.WorkingCount() = {m_swaqList.WorkingCount()} m_swaqList.waitingcount = {m_swaqList.WaitingData.Count}");
                if (count > 0)
                {
                    m_swaqList.Swap();//
                    var datas = m_swaqList.GetWaitingData();
                    foreach (var action in datas)
                    {
                        Profiler.BeginSample("action?.Invoke");
                        try
                        {
                            
                            action?.Invoke();
                            
                        }
                        catch (Exception e)
                        {
                            if (!s_isrun || (s_enum == WorkerStatus.Release || s_enum == WorkerStatus.GameQuit))
                            {
                                Debug.Log("Thread action worker over");
                                return;
                            }
                            else
                            {
                                Debug.LogError($"ThreadActionWorker Run action error = {e.ToString()}");
                            }
                        }
                        Profiler.EndSample();
                    }
                    m_swaqList.ClearWaitingData();
                }
                Profiler.EndSample();
                count= m_swaqList.GetWorkingLength();
                System.Threading.Thread.Sleep(count> 0 ?2:8);
            }
            Profiler.EndThreadProfiling();
        }
    }
}