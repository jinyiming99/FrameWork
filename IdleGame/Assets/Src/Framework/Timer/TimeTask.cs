using UnityEngine;
using System.Collections;
using System;
namespace GameFrameWork
{

    public class TimeTask
    {

        public Action m_fun = null;

        private float m_time = 0.0f;
        private float m_tagTime = 0.0f;
        private float m_updateCount = 0.0f;
        private float m_updateTime = 0.0f;
        private bool m_bIsStop = false;
        public Action m_over = null;

        public TimeTask()
        {
            
        }

        public void SetTask(Action fun, float time)
        {
            SetTask(fun, time, -1f, null);
        }
        
        public void SetTask(Action fun, float time, float updateTime)
        {
            SetTask(fun, time, updateTime, null);
        }
        
        public void SetTask(Action fun, float time, float updateTime, Action over)
        {
            m_fun = fun;
            m_time = 0.0f;
            m_tagTime = time;
            m_updateTime = updateTime;
            m_updateCount = 0.0f;
            m_bIsStop = false;
            m_over = over;
        }

        

        public bool Update(float time)
        {
            if (m_bIsStop)
                return false;
            m_time += time;
            m_updateCount += time;

            if (m_updateTime > 0f)
            {
                if (m_updateCount >= m_updateTime)
                {
                    if (m_fun != null)
                    {
                        try
                        {
                            m_fun();
                        }
                        catch (System.Exception ex)
                        {
                            DebugTools.DebugHelper.LogError("m_fun run error, eventName is " + m_fun.ToString() + "ex is " + ex);
                            return false;
                        }

                    }
                    m_updateCount -= m_updateTime;
                }

            }
            


            if (m_time >= m_tagTime && m_tagTime > 0.0f)
            {
                if (m_over != null)
                {
                    try
                    {
                        m_over();
                    }
                    catch (System.Exception ex)
                    {
                        DebugTools.DebugHelper.LogError("m_over run error, eventName is " + m_over.ToString() + "ex is " + ex);
                    }

                }
                return true;
            }
            return false;
        }

        public float GetToGetherPer()
        {
            return m_time / m_tagTime;
        }

        public float GetPassTime()
        {
            return m_time;
        }
        public float GetRemainingTime()
        {
            return m_tagTime - m_time;
        }

        public void NoUse()
        {
            m_fun = null;
            m_over = null;
        }
        public void Stop()
        {
            m_bIsStop = true;
        }
        public void Work()
        {
            m_bIsStop = false;
        }

    }
}
