using System;

namespace Core
{
    /// <summary>
    /// Internal listener entry with priority and weak reference support.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class ListenerEntry<T>
    {
        public Action<T> Delegate { get; }
        public EventPriority Priority { get; }
        public bool IsWeak { get; }
        public WeakReference WeakTarget { get; }
        public bool InvokeOnce { get; }

        public ListenerEntry(Action<T> del, EventPriority priority, bool isWeak,
            object target, bool invokeOnce)
        {
            Delegate = del;
            Priority = priority;
            IsWeak = isWeak;
            WeakTarget = target != null ? new WeakReference(target) : null;
            InvokeOnce = invokeOnce;
        }
    }
}