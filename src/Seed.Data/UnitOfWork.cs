using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Autofac;
using Seed.Common.Logging;

namespace Seed.Data
{
    public class UnitOfWork<TService1, TService2> : UnitOfWork, IUnitOfWork<TService1, TService2>
        where TService1 : class
        where TService2 : class
    {
        // C'tor
        public UnitOfWork(ILoggingService loggingService, ILifetimeScope container)
            : base(loggingService, container)
        {
        }


        [DebuggerStepThrough]
        public async Task<TResult> ExecuteAsync<TResult>(Func<TService1, TService2, Task<TResult>> func)
        {
            return await base.ExecuteAsync(async scope =>
            {
                var service1 = scope.Resolve<TService1>();
                var service2 = scope.Resolve<TService2>();

                return await func(service1, service2);
            });
        }
    }

    public class UnitOfWork<TService> : UnitOfWork, IUnitOfWork<TService> where TService : class
    {
        // C'tor
        public UnitOfWork(ILoggingService loggingService, ILifetimeScope container)
            : base(loggingService, container)
        {
        }


        // IUnitOfWorkFactory Members
        [DebuggerStepThrough]
        public async Task<TResult> ExecuteAsync<TResult>(Func<TService, Task<TResult>> func)
        {
            return await base.ExecuteAsync(async scope =>
            {
                var service = scope.Resolve<TService>();

                return await func(service);
            });
        }
    }

    public abstract class UnitOfWork
    {
        private readonly ILifetimeScope _container;

        // Instance variables
        private readonly ILoggingService _loggingService;


        // C'tor
        protected UnitOfWork(ILoggingService loggingService, ILifetimeScope container)
        {
            _loggingService = loggingService;
            _container = container;
        }


        // IUnitOfWorkFactory Members
        [DebuggerStepThrough]
        protected async Task<TResult> ExecuteAsync<TResult>(Func<ILifetimeScope, Task<TResult>> func)
        {
            using (var childScope = _container.BeginLifetimeScope())
            {
                var dbContext = childScope.Resolve<SeedToolsDbContext>();

                using (var transaction = await dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var result = await func(childScope);

                        await dbContext.SaveChangesAsync();
                        transaction.Commit();

                        return result;
                    }
                    catch (UnloggedUnitOfWorkException ex)
                    {
                        transaction.Rollback();
                        throw ex.WrappedException;
                    }
                    catch (Exception ex)
                    {
                        _loggingService.Error(ex, "Unhandled exception caught in unit of work. Rolling back.");

                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}