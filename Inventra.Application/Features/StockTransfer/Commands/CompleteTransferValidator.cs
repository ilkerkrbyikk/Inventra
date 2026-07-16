using FluentValidation;
using Inventra.Application.Features.StockTransfer.Commands;

namespace Inventra.Application.Features.StockTransfer.Validators
{
    public class CompleteTransferCommandValidator : AbstractValidator<CompleteTransferCommand>
    {
        public CompleteTransferCommandValidator()
        {
            RuleFor(x => x.TransactionId)
                .NotEmpty().WithMessage("Transaction ID is required.");

            RuleFor(x => x.TransferredQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Transferred quantity cannot be negative.");

            RuleFor(x => x.DefectiveQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Defective quantity cannot be negative.");
        }
    }
}