﻿using System;
using ProtoBuf;
using Ray.Core.EventSourcing;
using Ray.Core.Utils;

namespace Fan.IGrains.Account.Events
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public class AmountAddEvent : IEventBase<string>
    {
        #region base
        public string Id { get; set; }
        public Int64 Version { get; set; }
        public string TraceId { get; set; }
        public DateTime Timestamp { get; set; }
        public string StateId { get; set; }

        public string TypeCode => this.GetType().FullName;
        #endregion
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public AmountAddEvent() { }
        public AmountAddEvent(decimal amount, decimal balance)
        {
            Id = OGuid.GenerateNewId().ToString();
            Amount = amount;
            Balance = balance;
        }
    }
}
