using AutoMapper;
using CQRS.Application.CQRS.ProductCQRS.Command;
using CQRS.Application.CQRS.ProductCQRS.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CQRS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IMediator mediator;

        public ProductController(IMapper mapper, IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewProduct([FromQuery] CreateProductCommand command)
        {
            var result = await mediator.Send(command);
            
            if (result.Status == false)
                return BadRequest(result.Description);

            return Ok(result.ProductId);

        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery] GetAllProductsQuery request)
        {
            var products = await mediator.Send(request);
            return Ok(products.Products);
        }

        [HttpGet("Id")]
        public async Task<IActionResult> GetProductById([FromQuery] GetSingleProductQuery request)
        {
            var product = await mediator.Send(request);
            if (product == null)
                return NotFound();

            return Ok(product.Product);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProduct([FromQuery] DeleteProductCommand command)
        {
            var result = await mediator.Send(command);

            if (!result.Result)
                return BadRequest(result.Description);

            return Ok(result.Description);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateProduct([FromQuery] UpdateProductCommand command)
        {
            var result = await mediator.Send(command);

            if (!result.Result)
                return BadRequest(result.Description);

            return Ok(result.Description);
        }
    }
}
