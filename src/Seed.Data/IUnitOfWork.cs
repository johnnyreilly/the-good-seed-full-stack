using System;
using System.Threading.Tasks;

namespace Seed.Data
{
    public interface IUnitOfWork<out TService> where TService : class
    {
        Task<TResult> ExecuteAsync<TResult>(Func<TService, Task<TResult>> func);
    }

    public interface IUnitOfWork<out TService1, out TService2>
        where TService1 : class
        where TService2 : class
    {
        Task<TResult> ExecuteAsync<TResult>(Func<TService1, TService2, Task<TResult>> func);
    }
}