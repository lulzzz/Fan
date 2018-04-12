﻿using System;
using System.Threading.Tasks;
using Orleans;
using Ray.Core.EventSourcing;
using Fan.Grains.Account;
using Fan.IGrains;
using Fan.IGrains.Account.Actors;
using Fan.IGrains.Account.States;
using Ray.MongoDb;

namespace Fan.Grains.Account
{
    public sealed class AccountRep : MongoRepGrain<String, AccountState, MessageInfo>, IAccountRep
    {
        protected override string GrainId => this.GetPrimaryKeyString();

        static IEventHandle _eventHandle = new AccountEventHandle();
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
        protected override IEventHandle EventHandle => _eventHandle;

        public Task<decimal> GetBalance()
        {
            return Task.FromResult(State.Balance);
        }
    }
}
