using Orleans;
using Ray.Core.EventSourcing;

namespace Fan.IGrains.Account.Actors
{
    public interface IAccountDb : IAsyncGrain, IGrainWithStringKey
    {
    }
}
