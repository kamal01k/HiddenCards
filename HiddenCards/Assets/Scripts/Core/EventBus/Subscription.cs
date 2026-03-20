using System;

namespace Core
{
    /// <summary>
    /// Subscription handle for disposal.
    /// </summary>
    public class Subscription : IDisposable
    {
        private readonly Action _unsubscribe;
        private bool _disposed = false;

        public Subscription(Action unsubscribe)
        {
            _unsubscribe = unsubscribe;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _unsubscribe?.Invoke();
                _disposed = true;
            }
        }
    }
}