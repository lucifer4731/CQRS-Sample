using AutoMapper;
using CQRS.Application.Dto;
using CQRS.Domain.Entities;
using CQRS.Domain.IRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Application.CQRS.ProductCQRS.Query
{
    public class GetAllProductsQuery : IRequest<GetAllProductsQueryResponse>
    {
    }

    public class GetAllProductsQueryResponse
    {
        public List<ProductDto> Products { get; set; }
    }

    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, GetAllProductsQueryResponse>
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public GetAllProductsQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        public async Task<GetAllProductsQueryResponse> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Product> products = await productRepository.GetAllProductsAsync();

            var productDto = mapper.Map<List<ProductDto>>(products);
            GetAllProductsQueryResponse response = new GetAllProductsQueryResponse();
            response.Products = productDto.ToList();
            return response;
        }
    }
}
