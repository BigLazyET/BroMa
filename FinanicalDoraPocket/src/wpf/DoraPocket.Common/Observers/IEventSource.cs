using System;
using System.Threading.Tasks;

namespace DoraPocket.Common.Observers
{
    public interface IEventSource
    {
        IDisposable Subscribe(Func<string, object, Task> observer);

        void Fire(string eventName, object arguments);
    }
}
