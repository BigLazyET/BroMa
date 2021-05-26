using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoraPocket.Common.Observers
{
    /// <summary>
    /// 观察者模式：https://blog.csdn.net/zhgl7688/article/details/41969393
    /// </summary>
    public class EventSource : IEventSource
    {
        private readonly ConcurrentDictionary<Func<string, object, Task>, Func<string, object, Task>> observers;

        public EventSource()
        {
            observers = new ConcurrentDictionary<Func<string, object, Task>, Func<string, object, Task>>();
        }

        public void Fire(string eventName, object arguments)
        {
            Guard.ArgumentNotNullOrWhiteSpace(eventName, nameof(eventName));

            //Task.Run(async () => {
            //    foreach (var observer in _observers.Keys)
            //    {
            //        try
            //        {
            //            await observer(eventName, arguments);
            //        }
            //        catch (Exception ex)
            //        {
            //            //Log.Error(ex, "Unhandled error!");
            //        }
            //    }
            //});

            observers.Keys.AsParallel().ForAll(async observer =>
            {
                try
                {
                    await observer(eventName, arguments);
                }
                catch (Exception ex)
                {
                    //TODO
                }
            });
        }

        public IDisposable Subscribe(Func<string, object, Task> observer)
        {
            Guard.ArgumentNotNull(observer, nameof(observer));
            observers[observer] = observer;
            return new Disposable(() => observers.TryRemove(observer, out _));
        }

        private class Disposable : IDisposable
        {
            private readonly Action _action;

            public Disposable(Action action) => _action = action;

            public void Dispose() => _action();
        }
    }
}
