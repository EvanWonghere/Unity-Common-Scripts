using System;
using System.Collections.Generic;
using Utilities.Singleton;

namespace Utilities.EventSystem
{
    public class EventCenter : Singleton<EventCenter>
    {
        /// <summary>
        /// 事件中心
        /// </summary>
        /// <typeparam name="T"></typeparam>
        // 定义事件委托
        public delegate void EventHandler<in T>(T eventData);

        // 存储事件及其处理程序的字典
        private static readonly Dictionary<Type, Delegate> EventDictionary = new();

        // 注册事件
        public static void Register<T>(EventHandler<T> handler)
        {
            if (EventDictionary.TryGetValue(typeof(T), out var existingHandler))
            {
                EventDictionary[typeof(T)] = Delegate.Combine(existingHandler, handler);
            }
            else
            {
                EventDictionary[typeof(T)] = handler;
            }
        }

        // 注销事件
        public static void Unregister<T>(EventHandler<T> handler)
        {
            if (!EventDictionary.TryGetValue(typeof(T), out var existingHandler)) return;
            
            var newHandler = Delegate.Remove(existingHandler, handler);
            if (newHandler == null)
            {
                EventDictionary.Remove(typeof(T));
            }
            else
            {
                EventDictionary[typeof(T)] = newHandler;
            }
        }

        // 触发事件
        public static void Trigger<T>(T eventData)
        {
            if (EventDictionary.TryGetValue(typeof(T), out var handler))
            {
                ((EventHandler<T>)handler)?.Invoke(eventData);
            }
        }
    }
}