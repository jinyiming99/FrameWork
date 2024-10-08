using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace GameFrameWork.DebugTools
{
    /// <summary>
    /// debug用的对外的
    /// </summary>
    public static class DebugHelper
    {
        [Conditional("LOG_OUT")]
        public static void Log(string str)
        {
            if (string.IsNullOrEmpty(str))
                return;
            var logStr = $"log: time = {System.DateTime.Now.ToString()}, log = {str}";
            Debug.Log(logStr);

        }
        /// <summary>
        /// 为了防止在外面组装string，控制不了
        /// </summary>
        /// <param name="strFunc"></param>
        /// <param name="type"></param>
        [Conditional("LOG_OUT")]
        public static void Log(Func<string> strFunc)
        {
            string str = string.Empty;
            if (strFunc != null)
                str = strFunc();
            else
                return;
            Log(str);
        }
        [Conditional("LOG_OUT")]
        public static void LogWarning(string str)
        {
            if (string.IsNullOrEmpty(str))
                return;
            var logStr = $"warning: time = {System.DateTime.Now.ToString()}, log = {str}";
            Debug.LogWarning(logStr);

        }
        [Conditional("LOG_OUT")]
        public static void LogWarning(Func<string> strFunc)
        {
            string str = string.Empty;
            if (strFunc != null)
                str = strFunc();
            else
                return;

            LogWarning(str);
        }
        [Conditional("LOG_OUT")]
        public static void LogColor(string str,string color )
        {
            if (string.IsNullOrEmpty(str))
                return;
            var logStr = $"Error: time = {System.DateTime.Now.ToString()}, log = {string.Format("<color={2}> time ={0}   log :   </color> {1}", System.DateTime.Now.ToString() ,str, color)}";
            Debug.Log(logStr);
        }
        [Conditional("LOG_OUT")]
        public static void LogColor(Func<string> strFunc, string color)
        {
            string str = string.Empty;
            if (strFunc != null)
                str = strFunc();
            else
                return;

            LogColor(str, color);
        }
        [Conditional("LOG_OUT")]
        public static void LogError(string str)
        {
            if (string.IsNullOrEmpty(str))
                return;
            var logStr = $"Error time = {System.DateTime.Now.ToString()}, log = {str}";
            Debug.LogError(logStr);
        }
        [Conditional("LOG_OUT")]
        public static void LogError(Func<string> strFunc)
        {
            string str = string.Empty;
            if (strFunc != null)
                str = strFunc();
            else
                return;
            LogError(str);
        }

    }
}


