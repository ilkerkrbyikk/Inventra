using Inventra.Application.Features.StockTransfer.Commands;
using Inventra.Application.Features.StockTransfer.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Inventra.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockTransferController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StockTransferController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("request")]
        public async Task<IActionResult> CreateTransferRequest([FromBody] CreateTransferRequestCommand command)
        {
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPut("start")]
        public async Task<IActionResult> StartTransfer([FromBody] StartTransferCommand command)
        {
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPut("complete")]
        public async Task<IActionResult> CompleteTransfer([FromBody] CompleteTransferCommand command)
        {
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{transactionId}")]
        public async Task<IActionResult> GetTransaction(Guid transactionId)
        {
            var query = new GetTransactionByIdQuery { TransactionId = transactionId };
            var result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
    }
}