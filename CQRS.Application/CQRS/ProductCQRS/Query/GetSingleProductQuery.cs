using AutoMapper;
using CQRS.Application.Dto;
using CQRS.Domain.IRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Application.CQRS.ProductCQRS.Query
{
    public class GetSingleProductQuery : IRequest<GetSingleProductQueryResponse>
    {
        public Guid ProductId { get; set; }
    }

    public class GetSingleProductQueryResponse
    {
        public ProductDto Product { get; set; }
    }

    public class GetSingleProductQueryHandler : IRequestHandler<GetSingleProductQuery, GetSingleProductQueryResponse>
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public GetSingleProductQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
        }
        public async Task<GetSingleProductQueryResponse> Handle(GetSingleProductQuery request, CancellationToken cancellationToken)
        {
            var product = await productRepository.GetProductByIdAsync(request.ProductId);

            var productDto = mapper.Map<ProductDto>(product);
            GetSingleProductQueryResponse response = new GetSingleProductQueryResponse();
            response.Product = productDto;
            return response;
        }
    }
}
