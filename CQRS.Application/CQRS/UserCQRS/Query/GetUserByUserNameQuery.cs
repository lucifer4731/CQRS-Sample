using CQRS.Domain.Entities;
using CQRS.Domain.IRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Application.CQRS.UserCQRS.Query
{
    public class GetUserByUserNameQuery : IRequest<GetUserByUserNameQueryResponse>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class GetUserByUserNameQueryResponse
    {
        public User User { get; set; }
    }

    public class GetUserByUserNameQueryHandler : IRequestHandler<GetUserByUserNameQuery, GetUserByUserNameQueryResponse>
    {
        private readonly IUserRepository userRepository;

        public GetUserByUserNameQueryHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<GetUserByUserNameQueryResponse> Handle(GetUserByUserNameQuery request, CancellationToken cancellationToken)
        {
            GetUserByUserNameQueryResponse response = new GetUserByUserNameQueryResponse();
            var user = await userRepository.GetUserByUserNameAsync(request.UserName);

            if (user == null)
                return response;

            response.User = user;
            return response;
        }
    }
}
