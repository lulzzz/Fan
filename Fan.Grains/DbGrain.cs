using System;
using System.Threading.Tasks;
using Ray.Core.EventSourcing;
using Fan.IGrains;
using Ray.MongoDb;

namespace Fan.Grains
{
    public abstract class DbGrain<K, S> : MongoAsyncGrain<K, S, MessageInfo>
          where S : class, IState<K>, new()
    {
        protected override Task OnEventDelivered(IEventBase<K> @event)
        {
            return Process(@event);
        }

        protected abstract Task Process(IEventBase<K> @event);
    }
}
