using CQRS.Domain.IRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Application.CQRS.ProductCQRS.Command
{
    public class DeleteProductCommand : IRequest<DeleteProductCommandResponse>
    {
        public Guid ProductId { get; set; }
    }

    public class DeleteProductCommandResponse
    {
        public bool Result { get; set; }
        public string Description { get; set; }
    }

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, DeleteProductCommandResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IProductRepository productRepository;
        private readonly IUserRepository userRepository;

        public DeleteProductCommandHandler(IUnitOfWork unitOfWork, IProductRepository productRepository, IUserRepository userRepository)
        {
            this.unitOfWork = unitOfWork;
            this.productRepository = productRepository;
            this.userRepository = userRepository;
        }

        public async Task<DeleteProductCommandResponse> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            DeleteProductCommandResponse response = new DeleteProductCommandResponse();

            Guid loggedInUser = await userRepository.GetLoggedInUserIdAsync();
            Guid creatorUser = await productRepository.GetCreatorUserId(request.ProductId);
            if (loggedInUser != creatorUser)
            {
                response.Result = false;
                response.Description = "Only creator person can edit or delete product";
                return response;
            }

            await productRepository.DeleteProductAsync(request.ProductId);
            await unitOfWork.SaveChangesAsync();

            response.Result = true;
            response.Description = "successful";
            return response;
        }
    }
}