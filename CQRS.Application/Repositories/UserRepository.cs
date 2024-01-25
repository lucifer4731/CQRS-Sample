using CQRS.Domain.Entities;
using CQRS.Domain.IRepositories;
using CQRS.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Application.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SQLContext context;
        private readonly IUnitOfWork unitOfWork;

        public UserRepository(SQLContext context, IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.context = context;
        }

        public async Task<Guid> AddUserAsync(User user)
        {
            user.CreateDate = DateTime.Now;
            await context.Users.AddAsync(user);
            await unitOfWork.SaveChangesAsync();
            return user.Id;
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await context.Users.SingleOrDefaultAsync(u => u.UserName == userName);
        }
    }
}
