using CQRS.Domain.Entities;
using CQRS.Domain.IRepositories;
using CQRS.Infrastructure.Context;
using Microsoft.AspNetCore.Http;
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
        private readonly CQRSContext context;
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserRepository(CQRSContext context, IUnitOfWork unitOfWork,IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.httpContextAccessor = httpContextAccessor;
            this.context = context;
        }

        public async Task<Guid> AddUserAsync(User user)
        {
            user.CreateDate = DateTime.Now;
            await context.Users.AddAsync(user);
            return user.Id;
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await context.Users.SingleOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task<Guid> GetLoggedInUserIdAsync()
        {
            var userId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId").Value;
            Guid result = new Guid();
            Guid.TryParse(userId, out result);

            return await Task.FromResult(result);
        }
    }
}
