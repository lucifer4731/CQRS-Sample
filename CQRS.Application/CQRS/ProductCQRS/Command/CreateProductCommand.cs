using CQRS.Application.Repositories;
using CQRS.Domain.Entities;
using CQRS.Domain.IRepositories;
using CQRS.Infrastructure.PatternImplementations;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Application.CQRS.ProductCQRS.Command
{
    public class CreateProductCommand : IRequest<CreateProductCommandResponse>
    {
        public string Name { get; set; }
        public DateTime ProduceDate { get; set; }
        public string ManufacturePhone { get; set; }
        public string ManufactureEmail { get; set; }
        public bool IsAvailable { get; set; } = false;
        public string Description { get; set; }
    }

    public class CreateProductCommandResponse
    {
        public Guid ProductId { get; set; }
        public bool Status { get; set; }
        public string Description { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreateProductCommandResponse>
    {
        private readonly IProductRepository productRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserRepository userRepository;

        public CreateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            this.productRepository = productRepository;
            this.unitOfWork = unitOfWork;
            this.userRepository = userRepository;
        }
        public async Task<CreateProductCommandResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            CreateProductCommandResponse response = new CreateProductCommandResponse();
            var products = await productRepository.GetAllProductsAsync();
            bool checkEssentials = products.Any(p => p.ManufactureEmail == request.ManufactureEmail && p.ProduceDate == request.ProduceDate);
            if (checkEssentials)
            {
                response.ProductId = Guid.Empty;
                response.Status = false;
                response.Description = "duplicate ManufactureEmail and ProduceDate";
                return response;
            }

            Product product = new Product()
            {
                Id = Guid.NewGuid(),
                Title = request.Name,
                ProduceDate = request.ProduceDate,
                CreateDate = DateTime.UtcNow,
                Description = request.Description,
                Deleted = false,
                IsAvailable = request.IsAvailable,
                ManufactureEmail = request.ManufactureEmail,
                ManufacturePhone = request.ManufacturePhone,
                CreatorPerson = await userRepository.GetLoggedInUserIdAsync()
            };

            await productRepository.AddProductAsync(product);
            await unitOfWork.SaveChangesAsync();

            response.ProductId = product.Id;
            response.Status = true;
            response.Description = "successful";

            return response;
        }
    }
}
