using FluentValidation;
using Inventra.Application.Features.StockTransfer.Commands;

namespace Inventra.Application.Features.StockTransfer.Validators
{
    public class StartTransferCommandValidator : AbstractValidator<StartTransferCommand>
    {
        public StartTransferCommandValidator()
        {
            RuleFor(x => x.TransactionId)
                .NotEmpty().WithMessage("Transaction ID is required.");
        }
    }
}