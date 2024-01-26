using CQRS.Domain.IRepositories;
using CQRS.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Infrastructure.PatternImplementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CQRSContext context;

        public UnitOfWork(CQRSContext context)
        {
            this.context = context;
        }

        public void Dispose()
        {

        }
        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
