using Orleans;
using Ray.Core.EventSourcing;

namespace Fan.IGrains.Account.Actors
{
    public interface IAccountFlow : IAsyncGrain, IGrainWithStringKey
    {
    }
}
