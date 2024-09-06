using System;
using System.Collections.Concurrent;
using GameFrameWork.Containers;
using GameFrameWork.DebugTools;

namespace GameFrameWork.Thread
{
    public class ThreadWorker
    {
        enum WorkerEnum
        {
            None,
            Start,
            Stop,
            Over,
        }
        private SwapList<Action> m_workList = new SwapList<Action>();
        private System.Threading.Thread m_workThread;
        private WorkerEnum status;
        public ThreadWorker()
        {
            m_workThread = new System.Threading.Thread(Run);
            status = WorkerEnum.None;
        }

        public void Start()
        {
            m_workThread?.Start();
            status = WorkerEnum.Start;
        }

        public void Stop()
        {
            status = WorkerEnum.Stop;
            m_workThread?.Join();
            status = WorkerEnum.Over;
        }

        private void Run()
        {
            while (true)
            {
                if (status == WorkerEnum.Stop)
                    return;
                if (m_workList.GetWorkingLength() > 0)
                {
                    m_workList.Swap();
                    var list = m_workList.GetWaitingData();
                    foreach (var node in list)
                    {
                        if (status == WorkerEnum.Stop)
                            return;
                        try
                        {
                            node?.Invoke();
                        }
                        catch (Exception e)
                        {
                            DebugHelper.LogError(e.ToString());
                        }
                    }
                    m_workList.ClearWaitingData();
                }
                else
                {
                    System.Threading.Thread.Sleep(10);
                }
            }
        }
        public void Post(Action action)
        {
            m_workList.Add(action);
        }
    }
}