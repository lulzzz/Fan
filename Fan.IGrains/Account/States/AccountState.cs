﻿using ProtoBuf;
using Ray.Core.EventSourcing;
using System;

namespace Fan.IGrains.Account.States
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public class AccountState : IState<string>
    {
        #region base
        public string StateId { get; set; }
        public Int64 Version { get; set; }
        public Int64 DoingVersion { get; set; }
        public DateTime VersionTime { get; set; }
        #endregion
        public decimal Balance { get; set; }
    }
}
