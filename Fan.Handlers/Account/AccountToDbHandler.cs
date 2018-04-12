using Ray.Core.Message;
using Ray.RabbitMQ;
using System.Threading.Tasks;
using Fan.IGrains;
using System;
using Ray.Core.MQ;
using Ray.Core;
using Fan.IGrains.Account.Actors;

namespace Fan.Handlers.Account
{
    [RabbitSub("Read", "Account", "account")]
    public sealed class AccountToDbHandler : SubHandler<string, MessageInfo>
    {
        private readonly IOrleansClientFactory _clientFactory;
        public AccountToDbHandler(IServiceProvider svProvider, IOrleansClientFactory clientFactory) : base(svProvider)
        {
            this._clientFactory = clientFactory;
        }
        public override Task Tell(byte[] bytes, IActorOwnMessage<string> data, MessageInfo msg)
        {
            return _clientFactory.GetClient().GetGrain<IAccountDb>(data.StateId).Tell(bytes);
        }
    }
}
