using System;
using System.Collections.Generic;
using Define;

namespace IEvent
{
    public class IEventCenter
    {
        static Dictionary<EventID, List<Delegate>> mEvent = new Dictionary<EventID, List<Delegate>>(new Define.IEventTypeComparer());

        static void OnListenerAdding(EventID e,Delegate d)
        {
            List<Delegate> list = null;
            mEvent.TryGetValue(e, out list);
            if(null == list)
            {
                list = new List<Delegate>();
                mEvent.Add(e, list);
            }

            for(int i = 0; i < list.Count; i++)
            {
                if(list[i] == d)
                {
                    string error = string.Format("添加事件监听错误， EventID：{0}， 添加的事件{1}", e.ToString(), d.GetType().Name);
                    UnityEngine.Debug.LogError(error);
                    return;
                }
            }
            list.Add(d);
        }
        static void OnListenerRemove(EventID e,Delegate d)
        {
            List<Delegate> list = null;
            mEvent.TryGetValue(e, out list);
            if(null == null || list.Count == 0)
            {
                return;
            }
            for(int i = 0; i < list.Count; i++)
            {
                if(list[i] == d)
                {
                    mEvent.Remove(e);
                    break;
                }
            }
        }

        public static void AddHandler(EventID e,Callback handler)
        {
            OnListenerAdding(e, handler);
        }
        public static void AddHandler<T>(EventID e,Callback<T> handler)
        {
            OnListenerAdding(e, handler);
        }
        public static void AddHandler<T, U>(EventID e, Callback<T, U> handler)
        {
            OnListenerAdding(e, handler);
        }
        public static void AddHandler<T, U, V>(EventID e, Callback<T, U, V> handler)
        {
            OnListenerAdding(e, handler);
        }
        public static void AddHandler<T, U, V, X>(EventID e,Callback<T, U, V, X> handler)
        {
            OnListenerAdding(e, handler);
        }

        public static void DelHandler(EventID e,Callback handler)
        {
            OnListenerRemove(e, handler);
        }
        public static void DelHandler<T>(EventID e, Callback<T> handler)
        {
            OnListenerRemove(e, handler);
        }
        public static void DelHandler<T, U>(EventID e, Callback<T, U> handler)
        {
            OnListenerRemove(e, handler);
        }
        public static void DelHandler<T, U, V>(EventID e, Callback<T, U, V> handler)
        {
            OnListenerRemove(e, handler);
        }
        public static void DelHandler<T, U, V, X>(EventID e, Callback<T, U, V, X> handler)
        {
            OnListenerRemove(e, handler);
        }

        public static void SendEvent(EventID e)
        {
            List<Delegate> list = null;
            if(mEvent.TryGetValue(e,out list))
            {
                for(int i = 0; i < list.Count; i++)
                {
                    Delegate d = list[i];
                    Callback callback = d as Callback;
                    if(callback != null)
                    {
                        callback();
                    }
                }
            }
        }

        public static void SendEvent<T>(EventID e, T arg1)
        {
            List<Delegate> list = null;
            if(mEvent.TryGetValue(e,out list))
            {
                for(int i = 0; i < list.Count; i++)
                {
                    Delegate d = list[i];
                    Callback<T> callback = d as Callback<T>;
                    if(callback != null)
                    {
                        callback(arg1);
                    }                }
            }
        }

        public static void SendEvent<T, U>(EventID e, T arg1, U arg2)
        {
            List<Delegate> list = null;
            if(mEvent.TryGetValue(e,out list))
            {
                for(int i = 0; i < list.Count; i++)
                {
                    Delegate d = list[i];
                    Callback<T, U> callback = d as Callback<T, U>;
                    if(callback != null)
                    {
                        callback(arg1, arg2);
                    }
                }
            }
        }

        public static void SendEvent<T, U, V>(EventID e, T arg1, U arg2, V arg3)
        {
            List<Delegate> list = null;
            if(mEvent.TryGetValue(e,out list))
            {
                for(int i = 0; i < list.Count; i++)
                {
                    Delegate d = list[i];
                    Callback<T, U, V> callback = d as Callback<T, U, V>;
                    if (callback != null)
                    {
                        callback(arg1, arg2, arg3);
                    }
                }
            }
        }

        public static void SendEvent<T, U, V, X>(EventID e,T arg1, U arg2, V arg3, X arg4)
        {
            List<Delegate> list = null;
            if(mEvent.TryGetValue(e,out list))
            {
                for(int i = 0; i < list.Count; i++)
                {
                    Delegate d = list[i];
                    Callback<T, U, V, X> callback = d as Callback<T, U, V, X>;
                    if(callback != null)
                    {
                        callback(arg1, arg2, arg3, arg4);
                    }
                }
            }
        }
    }
}