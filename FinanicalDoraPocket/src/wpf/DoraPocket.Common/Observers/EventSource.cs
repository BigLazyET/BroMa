using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace DoraPocket.Common.Observers
{
    /// <summary>
    /// 观察者模式：https://blog.csdn.net/zhgl7688/article/details/41969393
    /// </summary>
    public class EventSource : IEventSource
    {
        private readonly ConcurrentDictionary<string, Func<object, Task>> observers;

        public EventSource()
        {
            observers = new ConcurrentDictionary<string, Func<object, Task>>();
        }

        public void Fire(string eventName, object arguments)
        {
            Guard.ArgumentNotNullOrWhiteSpace(eventName, nameof(eventName));

            if (!observers.ContainsKey(eventName))
            {
                throw new ArgumentException($"eventName '{eventName}' is not subscribe before");
            }
            var observer = observers[eventName];
            observer(arguments);
        }

        public bool CanSubscribe(string eventName)
        {
            if (observers.ContainsKey(eventName))
                return false;
            return true;
        }

        public IDisposable Subscribe(string eventName, Func<object, Task> observer)
        {
            Guard.ArgumentNotNull(eventName, nameof(eventName));
            Guard.ArgumentNotNull(observer, nameof(observer));
            if (observers.ContainsKey(eventName))
            {
                //throw new ArgumentException($"eventName '{eventName}' is already subscribe before");
                return new Disposable(null);
            }
            observers[eventName] = observer;
            return new Disposable(() => observers.TryRemove(eventName, out _));
        }

        private class Disposable : IDisposable
        {
            private readonly Action _action;

            public Disposable(Action action) => _action = action;

            public void Dispose() => _action();
        }
    }
}
