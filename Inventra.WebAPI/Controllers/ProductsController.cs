using Inventra.Application.Features.Products.Commands;
using Inventra.Application.Features.Products.Queries;
using Inventra.WebAPI.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Inventra.WebAPI.Controllers
{
    /// <summary>
    /// API Controller for product operations.
    /// Handles CRUD operations for products.
    /// All requests are dispatched via MediatR, validated by FluentValidation,
    /// and responses are mapped from Result to HTTP status codes.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get a product by ID.
        /// </summary>
        /// <param name="id">The product ID.</param>
        /// <returns>Product details if found, 404 if not.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetProductByIdQuery(id);
            var result = await _mediator.Send(query);
            return result.ToHttpResponse();
        }

        /// <summary>
        /// Get all products with pagination.
        /// </summary>
        /// <param name="pageNumber">Page number (default: 1).</param>
        /// <param name="pageSize">Page size (default: 100).</param>
        /// <returns>List of products.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100)
        {
            var query = new GetAllProductsQuery(pageNumber, pageSize);
            var result = await _mediator.Send(query);
            return result.ToHttpResponse();
        }

        /// <summary>
        /// Create a new product.
        /// </summary>
        /// <param name="command">Product creation data.</param>
        /// <returns>Created product ID, 201 Created.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ToHttpResponse(StatusCodes.Status201Created);
        }

        /// <summary>
        /// Update an existing product.
        /// Null values in the request body are ignored (partial update).
        /// </summary>
        /// <param name="id">The product ID.</param>
        /// <param name="command">Product update data.</param>
        /// <returns>200 OK on success, 404 if not found.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductCommand command)
        {
            // Merge ID from route into command
            var mergedCommand = new UpdateProductCommand(id, command.Name, command.Price, command.StockQuantity);
            var result = await _mediator.Send(mergedCommand);
            return result.ToHttpResponse();
        }

        /// <summary>
        /// Delete a product (soft delete).
        /// </summary>
        /// <param name="id">The product ID.</param>
        /// <returns>200 OK on success, 404 if not found.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteProductCommand(id);
            var result = await _mediator.Send(command);
            return result.ToHttpResponse();
        }
    }
}