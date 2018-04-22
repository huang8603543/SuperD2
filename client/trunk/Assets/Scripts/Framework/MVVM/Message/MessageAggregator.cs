using System.Collections.Generic;
using Framework.Util;
using System;

namespace Framework.MVVM
{
    public class MessageAggregator<T> : Singleton<MessageAggregator<T>>
    {
        private readonly Dictionary<string, Action<object, MessageArgs<T>>> messages = new Dictionary<string, Action<object, MessageArgs<T>>>();

        public void Subscribe(string name, Action<object, MessageArgs<T>> handler)
        {
            if (!messages.ContainsKey(name))
            {
                messages.Add(name, handler);
            }
            else
            {
                messages[name] += handler;
            }
        }

        public void Publish(string name, object sender, MessageArgs<T> args)
        {
            if (messages.ContainsKey(name) && messages[name] != null)
            {
                messages[name](sender, args);
            }
        }
    }
}
