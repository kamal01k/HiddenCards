using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Core
{
    /// <summary>
    /// Main Message Center Class.
    /// </summary>
    public static partial class MessageCenter
    {
        private static readonly ConcurrentDictionary<string, object> _listeners = new ConcurrentDictionary<string, object>();
        private static readonly ConcurrentDictionary<string, object> _onceListeners = new ConcurrentDictionary<string, object>();

        #region Core API

        /// <summary>
        /// Add Listener with Priority and Weak Reference support.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        /// <param name="listener"></param>
        /// <param name="priority"></param>
        /// <param name="weak"></param>
        /// <returns></returns>
        public static Subscription AddListener<T>(MsgID<T> msg, Action<T> listener,
            EventPriority priority = EventPriority.Normal, bool weak = false)
        {
            var entry = new ListenerEntry<T>(listener, priority, weak, listener.Target, false);
            AddToDictionary(_listeners, msg.Key, entry);
            return new Subscription(() => RemoveListener(msg, listener));
        }

        /// <summary>
        /// Add Once Listener (auto-remove after first call).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        /// <param name="listener"></param>
        /// <param name="priority"></param>
        /// <param name="weak"></param>
        /// <returns></returns>
        public static Subscription AddListenerOnce<T>(MsgID<T> msg, Action<T> listener,
            EventPriority priority = EventPriority.Normal, bool weak = false)
        {
            var entry = new ListenerEntry<T>(listener, priority, weak, listener.Target, true);
            AddToDictionary(_onceListeners, msg.Key, entry);
            return new Subscription(() => RemoveOnceListener(msg, listener));
        }

        /// <summary>
        /// Add Void Listener.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="listener"></param>
        /// <param name="priority"></param>
        /// <param name="weak"></param>
        /// <returns></returns>
        public static Subscription AddListener(MsgID<Void> msg, Action listener,
            EventPriority priority = EventPriority.Normal, bool weak = false)
        {
            var entry = new ListenerEntry<Void>(param => listener(), priority, weak, listener.Target, false);
            AddToDictionary(_listeners, msg.Key, entry);
            return new Subscription(() => RemoveListener(msg, listener));
        }

        /// <summary>
        /// Add Void Once Listener.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="listener"></param>
        /// <param name="priority"></param>
        /// <param name="weak"></param>
        /// <returns></returns>
        public static Subscription AddListenerOnce(MsgID<Void> msg, Action listener,
            EventPriority priority = EventPriority.Normal, bool weak = false)
        {
            var entry = new ListenerEntry<Void>(param => listener(), priority, weak, listener.Target, true);
            AddToDictionary(_onceListeners, msg.Key, entry);
            return new Subscription(() => RemoveOnceListener(msg, listener));
        }

        /// <summary>
        /// Send message with parameter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        /// <param name="param"></param>
        public static void Send<T>(MsgID<T> msg, T param)
        {
            InvokeListeners(msg.Key, param, _listeners);
            InvokeListeners(msg.Key, param, _onceListeners, true);
        }

        /// <summary>
        /// Send Void message.
        /// </summary>
        /// <param name="msg"></param>
        public static void Send(MsgID<Void> msg)
        {
            InvokeListeners(msg.Key, Void.Value, _listeners);
            InvokeListeners(msg.Key, Void.Value, _onceListeners, true);
        }

        /// <summary>
        /// Remove specific listener.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        /// <param name="listener"></param>
        public static void RemoveListener<T>(MsgID<T> msg, Action<T> listener)
        {
            RemoveFromDictionary(_listeners, msg.Key, listener);
        }

        public static void RemoveListener(MsgID<Void> msg, Action listener)
        {
            RemoveFromDictionary(_listeners, msg.Key, (Void _) => listener());
        }

        /// <summary>
        /// Remove once listener.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        /// <param name="listener"></param>
        public static void RemoveOnceListener<T>(MsgID<T> msg, Action<T> listener)
        {
            RemoveFromDictionary(_onceListeners, msg.Key, listener);
        }

        public static void RemoveOnceListener(MsgID<Void> msg, Action listener)
        {
            RemoveFromDictionary(_onceListeners, msg.Key, (Void _) => listener());
        }

        /// <summary>
        /// Remove all listeners for a message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        public static void RemoveListener<T>(MsgID<T> msg)
        {
            _listeners.TryRemove(msg.Key, out _);
            _onceListeners.TryRemove(msg.Key, out _);
        }

        #endregion

        #region Internal Implementation

        private static void AddToDictionary<T>(ConcurrentDictionary<string, object> dict, string key, ListenerEntry<T> entry)
        {
            dict.AddOrUpdate(key,
                _ => new List<ListenerEntry<T>> { entry },
                (_, existing) =>
                {
                    var list = (List<ListenerEntry<T>>)existing;
                    lock (list)
                    {
                        list.Add(entry);
                        list.Sort((a, b) => a.Priority.CompareTo(b.Priority));
                    }
                    return list;
                });
        }

        private static void RemoveFromDictionary<T>(ConcurrentDictionary<string, object> dict, string key, Action<T> listener)
        {
            if (dict.TryGetValue(key, out var existing))
            {
                var list = (List<ListenerEntry<T>>)existing;
                lock (list)
                {
                    list.RemoveAll(e => e.Delegate == listener ||
                                      (e.WeakTarget != null && !e.WeakTarget.IsAlive));
                    if (list.Count == 0) dict.TryRemove(key, out _);
                }
            }
        }

        private static void InvokeListeners<T>(string key, T param,
            ConcurrentDictionary<string, object> source, bool removeAfterInvoke = false)
        {
            if (!source.TryGetValue(key, out var existing)) return;

            var list = (List<ListenerEntry<T>>)existing;
            List<ListenerEntry<T>>? toRemove = null;

            // Create snapshot to avoid modification during iteration
            ListenerEntry<T>[] snapshot;
            lock (list) { snapshot = list.ToArray(); }

            foreach (var entry in snapshot)
            {
                try
                {
                    // Check weak reference validity
                    if (entry.IsWeak && entry.WeakTarget != null)
                    {
                        if (!entry.WeakTarget.IsAlive)
                        {
                            (toRemove ??= new List<ListenerEntry<T>>()).Add(entry);
                            continue;
                        }
                    }

                    entry.Delegate(param);

                    if (removeAfterInvoke || entry.InvokeOnce)
                    {
                        (toRemove ??= new List<ListenerEntry<T>>()).Add(entry);
                    }
                }
                catch (Exception ex)
                {
#if DEBUG
                    System.Console.WriteLine($"EventBus Error [{key}]: {ex.Message}");
#endif
                }
            }

            // Cleanup dead/once listeners
            if (toRemove != null)
            {
                lock (list)
                {
                    foreach (var dead in toRemove) list.Remove(dead);
                    if (list.Count == 0) source.TryRemove(key, out _);
                }
            }
        }

        #endregion
    }
}