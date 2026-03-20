using System;

namespace Core
{
    /// <summary>
    /// Message ID class. Strongly-typed identifier.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MsgID<T>
    {
        public string Key { get; }

        public MsgID(string key)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
        }

        public static implicit operator MsgID<T>(string key) => new MsgID<T>(key);
    }
}