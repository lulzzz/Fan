﻿using System.Threading.Tasks;
using Orleans;
using Ray.Core.EventSourcing;
using Fan.IGrains.Account.Actors;
using Fan.IGrains.Account.States;
using Fan.IGrains.Account.Events;
using Ray.MongoDb;
using Orleans.Concurrency;
using Ray.RabbitMQ;
using Fan.Grains.Account;

namespace Fan.Grain.Account
{
    [RabbitPub("Account", "account")]
    public sealed class Account : MongoGrain<string, AccountState, IGrains.MessageInfo>, IAccount
    {
        protected override string GrainId => this.GetPrimaryKeyString();

        static IEventHandle _eventHandle = new AccountEventHandle();
        protected override IEventHandle EventHandle => _eventHandle;
        static MongoGrainConfig _ESMongoInfo;
        public override MongoGrainConfig GrainConfig
        {
            get
            {
                if (_ESMongoInfo == null)
                    _ESMongoInfo = new MongoGrainConfig("Test", "Account_Event", "Account_State");
                return _ESMongoInfo;
            }
        }

        public override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();
        }
        public Task Transfer(string toAccountId, decimal amount)
        {
            var evt = new AmountTransferEvent(toAccountId, amount, this.State.Balance - amount);
            return RaiseEvent(evt).AsTask();
        }
        public Task AddAmount(decimal amount, string uniqueId = null)
        {
            var evt = new AmountAddEvent(amount, this.State.Balance + amount);
            return RaiseEvent(evt, uniqueId: uniqueId).AsTask();
        }
        [AlwaysInterleave]
        public Task<decimal> GetBalance()
        {
            return Task.FromResult(State.Balance);
        }
    }
}
