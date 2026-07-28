using FluentValidation;

namespace Inventra.Application.Features.StockTransfer.Commands
{
    /// <summary>
    /// Validator for StartTransferCommand.
    /// Ensures the transaction ID is valid.
    /// </summary>
    public class StartTransferCommandValidator : AbstractValidator<StartTransferCommand>
    {
        public StartTransferCommandValidator()
        {
            RuleFor(x => x.TransactionId)
                .NotEqual(Guid.Empty)
                .WithMessage("Transaction ID cannot be empty.");
        }
    }
}