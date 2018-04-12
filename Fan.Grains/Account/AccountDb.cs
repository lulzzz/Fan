using System;
using System.Threading.Tasks;
using Orleans;
using Ray.Core.EventSourcing;
using Fan.IGrains.Account.Actors;
using Fan.IGrains.Account.Events;
using Fan.IGrains.Account.States;
using Ray.MongoDb;

namespace Fan.Grains.Account
{
    public sealed class AccountDb : DbGrain<string, AsyncState<string>>, IAccountDb
    {
        protected override string GrainId => this.GetPrimaryKeyString();
        static MongoGrainConfig _ESMongoInfo;
        public override MongoGrainConfig GrainConfig
        {
            get
            {
                if (_ESMongoInfo == null)
                    _ESMongoInfo = new MongoGrainConfig("Test", "Account_Event", "Account_Db_State");
                return _ESMongoInfo;
            }
        }
        protected override Task Process(IEventBase<string> @event)
        {
            switch (@event)
            {
                case AmountAddEvent value: return AmountAddEventHandler(value);
                case AmountTransferEvent value: return AmountTransferEventHandler(value);
                default: return Task.CompletedTask;
            }
        }
        public Task AmountTransferEventHandler(AmountTransferEvent evt)
        {
            Console.WriteLine($"更新数据库->用户转账,当前账户ID:{evt.StateId},目标账户ID:{evt.ToAccountId},转账金额:{evt.Amount},当前余额为:{evt.Balance}");
            return Task.CompletedTask;
        }
        public Task AmountAddEventHandler(AmountAddEvent evt)
        {
            Console.WriteLine($"更新数据库->用户转账到账,用户ID:{evt.StateId},到账金额:{evt.Amount},当前余额为:{evt.Balance}");
            return Task.CompletedTask;
        }
    }
}
