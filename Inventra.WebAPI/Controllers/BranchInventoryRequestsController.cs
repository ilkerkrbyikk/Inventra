using Inventra.Application.Features.BranchInventoryRequests.Commands;
using Inventra.WebAPI.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Inventra.WebAPI.Controllers
{
    /// <summary>
    /// API Controller for branch inventory request operations.
    ///
    /// A branch inventory request is a formal stock request from a branch to a warehouse.
    /// If the requested quantity meets or exceeds the configured large-request threshold,
    /// the warehouse manager is notified in real time via SignalR (wired in a later phase).
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BranchInventoryRequestsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BranchInventoryRequestsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Submit a new stock request from a branch to the warehouse.
        /// </summary>
        /// <param name="command">Request details including branch, product, warehouse, and quantity.</param>
        /// <returns>201 Created with the new request ID, or 400 Bad Request on validation failure.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateBranchInventoryRequestCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ToHttpResponse(StatusCodes.Status201Created);
        }
    }
}
