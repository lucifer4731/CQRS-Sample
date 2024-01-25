using CQRS.Application.Utilities;
using CQRS.Domain.Entities;
using CQRS.Domain.IRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Application.CQRS.UserCQRS.Command
{
    public class CreateUserCommand : IRequest<CreateUserCommandResponse>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? Description { get; set; }
    }

    public class CreateUserCommandResponse
    {
        public Guid UserId { get; set; }
    }


    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserCommandResponse>
    {
        private readonly EncryptionUtility encryptionUtility;
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;

        public CreateUserCommandHandler(EncryptionUtility encryptionUtility, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            this.encryptionUtility = encryptionUtility;
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var salt = encryptionUtility.GetNewSalt();
                var hashPassword = encryptionUtility.GetSHA256(request.Password, salt);

                User user = new User
                {
                    Id = Guid.NewGuid(),
                    FullName = request.FirstName + " " + request.LastName,
                    UserName = request.UserName,
                    Password = hashPassword,
                    Description = request.Description,
                    CreateDate = DateTime.Now
                };

                await userRepository.AddUserAsync(user);
                await unitOfWork.SaveChangesAsync();
                var response = new CreateUserCommandResponse()
                {
                    UserId = user.Id
                };
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
