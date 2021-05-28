using System;
using System.Threading.Tasks;

namespace DoraPocket.Common.Observers
{
    public interface IEventSource
    {
        IDisposable Subscribe(string eventName, Func<object, Task> observer);

        void Fire(string eventName, object arguments);
    }
}
