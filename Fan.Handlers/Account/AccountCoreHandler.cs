using Ray.Core;
using Ray.Core.EventSourcing;
using Ray.Core.MQ;
using Fan.IGrains;
using Fan.IGrains.Account.Actors;
using Ray.RabbitMQ;
using System;
using System.Threading.Tasks;

namespace Fan.Handlers.Account
{
    [RabbitSub("Core", "Account", "account")]
    public sealed class AccountCoreHandler : MultHandler<string, MessageInfo>
    {
        private readonly IOrleansClientFactory _clientFactory;
        public AccountCoreHandler(IServiceProvider svProvider, IOrleansClientFactory clientFactory) : base(svProvider)
        {
            this._clientFactory = clientFactory;
        }

        protected override Task SendToAsyncGrain(byte[] bytes, IEventBase<string> evt)
        {
            var client = _clientFactory.GetClient();
            return Task.WhenAll(
                client.GetGrain<IAccountRep>(evt.StateId).Tell(bytes),
                client.GetGrain<IAccountFlow>(evt.StateId).Tell(bytes)
                );
        }
    }
}
