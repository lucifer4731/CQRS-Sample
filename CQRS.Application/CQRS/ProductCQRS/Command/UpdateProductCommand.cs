using CQRS.Domain.Entities;
using CQRS.Domain.IRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Application.CQRS.ProductCQRS.Command
{
    public class UpdateProductCommand : IRequest<UpdateProductCommandResponse>
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public DateTime ProduceDate { get; set; }
        public string ManufacturePhone { get; set; }
        public string ManufactureEmail { get; set; }
        public bool IsAvailable { get; set; } = false;
        public string Description { get; set; }
    }

    public class UpdateProductCommandResponse
    {
        public bool Result { get; set; }
        public string Description { get; set; }
    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, UpdateProductCommandResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IProductRepository productRepository;
        private readonly IUserRepository userRepository;

        public UpdateProductCommandHandler(IUnitOfWork unitOfWork, IProductRepository productRepository, IUserRepository userRepository)
        {
            this.unitOfWork = unitOfWork;
            this.productRepository = productRepository;
            this.userRepository = userRepository;
        }

        public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            UpdateProductCommandResponse response = new UpdateProductCommandResponse();

            Guid loggedInUser = await userRepository.GetLoggedInUserIdAsync();
            Guid creatorUser = await productRepository.GetCreatorUserId(request.ProductId);
            if (loggedInUser != creatorUser)
            {
                response.Result = false;
                response.Description = "Only creator person can edit or delete product";
                return response;
            }

            Product product = new Product();
            product.Id = request.ProductId;
            product.ManufacturePhone = request.ManufacturePhone;
            product.ManufactureEmail = request.ManufactureEmail;
            product.ProduceDate = request.ProduceDate;
            product.LastUpdateDate = DateTime.UtcNow;
            product.IsAvailable = request.IsAvailable;
            product.Description = request.Description;
            product.Title = request.Name;

            productRepository.UpdateProduct(product, request.ProductId);
            await unitOfWork.SaveChangesAsync();

            response.Result = true;
            response.Description = "successful";
            return response;
        }
    }
}
