using UnityEngine;
using System.Collections.Generic;
using GameFrameWork.Pool;
using UnityEditor.VersionControl;

namespace GameFrameWork.Timer
{
    public class TimeTaskManager 
    {
        private List<TimeTask> m_taskList = null;
        private List<TimeTask> m_willRemove = null;
        private List<TimeTask> m_willAdd = null;

        public TimeTask AddTask(System.Action fun, float time)
        {
            TimeTask task = CreateTask();
            task.SetTask(fun,time);
            m_willAdd.Add(task);
            return task;
        }
        public TimeTask AddTask(System.Action fun, float time, float updateTime)
        {
            TimeTask task = CreateTask();
            task.SetTask(fun, time, updateTime);
            m_willAdd.Add(task);
            return task;
        }

        public TimeTask AddTask(System.Action fun, float time, float updateTime, System.Action over)
        {
            TimeTask task = CreateTask();
            task.SetTask(fun, time, updateTime,over);
            m_willAdd.Add(task);
            return task;
        }

        private TimeTask CreateTask()
        {
            TimeTask task = ClassPool<TimeTask>.Spawn();
            if (task is null)
                task = new TimeTask();
            return task;
        }

        public void RemoveTask(TimeTask task)
        {
            if (task != null)
            {
                m_willRemove.Add(task);
            }
            // if (m_taskList.Contains(task))
            //    m_taskList.Remove(task);
        }
        // Use this for initialization
        public TimeTaskManager()
        {
            m_taskList = new List<TimeTask>();
            m_willRemove = new List<TimeTask>();
            m_willAdd = new List<TimeTask>();
        }

        // Update is called once per frame
        public void Update(float deltaTime)
        {
            if ((m_taskList.Count == 0 && m_willAdd.Count == 0))
                return;
            int length = m_willAdd.Count;
            //防止update的时候添加出错
            if (length > 0)
            {
                for( int i = 0; i < length;i++)
                {
                    TimeTask task = m_willAdd[i];
                    m_taskList.Add(task);
                }
                m_willAdd.Clear();
            }
            length = m_willRemove.Count;
            if (length > 0)
            {
                //移除无用的
                for (int i = 0; i < length; i++)
                {
                    TimeTask task = m_willRemove[i];
                    if (m_taskList.Contains(task))
                    {
                        m_taskList.Remove(task);
                    }
                    ClassPool<TimeTask>.Despawn(task);
                }
                m_willRemove.Clear();
            }

            //更新时间
            float time = deltaTime ;
            length = m_taskList.Count;
            if (length > 0)
            {
                for (int i = 0; i < length; i++)
                {
                    TimeTask task = m_taskList[i];
                    if (task.Update(time))
                        m_willRemove.Add(task);
                }
            }
        }

        public void Release()
        {
            m_willAdd.Clear();
            m_willRemove.Clear();
            m_taskList.Clear();
        }
        public void Clear()
        {
            m_willAdd.Clear();
            m_willRemove.Clear();
            m_taskList.Clear();
        }
    }
}
