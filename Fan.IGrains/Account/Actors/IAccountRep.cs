using Orleans;
using System.Threading.Tasks;
using Ray.Core.EventSourcing;

namespace Fan.IGrains.Account.Actors
{
    public interface IAccountRep : IAsyncGrain, IGrainWithStringKey
    {
        /// <summary>
        /// 获取账户余额
        /// </summary>
        /// <returns></returns>
        Task<decimal> GetBalance();
    }
}
