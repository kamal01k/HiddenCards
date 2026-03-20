using System.Threading.Tasks;

namespace Core
{
    /// <summary>
    /// Async EventBus Extensions.
    /// </summary>
    public static partial class MessageCenter
    {
        /// <summary>
        /// Send asynchronously.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static async Task SendAsync<T>(MsgID<T> msg, T param)
        {
            await Task.Run(() => Send(msg, param));
        }

        /// <summary>
        /// Send Void asynchronously.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static async Task SendAsync(MsgID<Void> msg)
        {
            await Task.Run(() => Send(msg));
        }

        /// <summary>
        /// Send on main thread (Unity-specific).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        /// <param name="param"></param>
        public static void SendOnMainThread<T>(MsgID<T> msg, T param)
        {
            // In Unity, you would queue this to the main thread
            // For example: UnityEngine.UnitySynchronizationContext.QueueOnMainThread(() => Send(msg, param));
            Send(msg, param);
        }

        /// <summary>
        /// Send Void on main thread.
        /// </summary>
        /// <param name="msg"></param>
        public static void SendOnMainThread(MsgID<Void> msg)
        {
            SendOnMainThread(msg, Void.Value);
        }
    }
}