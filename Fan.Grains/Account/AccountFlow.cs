﻿using System;
using System.Threading.Tasks;
using Orleans;
using Ray.Core.EventSourcing;
using Fan.IGrains;
using Fan.IGrains.Account.Actors;
using Fan.IGrains.Account.Events;
using Fan.IGrains.Account.States;
using Ray.MongoDb;

namespace Fan.Grain
{
    public sealed class AccountFlow : MongoAsyncGrain<string, AsyncState<string>, MessageInfo>, IAccountFlow
    {
        protected override string GrainId => this.GetPrimaryKeyString();
        static MongoGrainConfig _ESMongoInfo;
        public override MongoGrainConfig GrainConfig
        {
            get
            {
                if (_ESMongoInfo == null)
                    _ESMongoInfo = new MongoGrainConfig("Test", "Account_Event", "Account_Flow_State");
                return _ESMongoInfo;
            }
        }
        protected override Task OnEventDelivered(IEventBase<string> @event)
        {
            switch (@event)
            {
                case AmountTransferEvent value: return AmountAddEventHandler(value);
                default: return Task.CompletedTask;
            }
        }
        public async Task AmountAddEventHandler(AmountTransferEvent value)
        {
            var toActor = GrainFactory.GetGrain<IAccount>(value.ToAccountId);
            var balance = await toActor.GetBalance();
            await toActor.AddAmount(value.Amount, value.Id);
        }
    }
}
