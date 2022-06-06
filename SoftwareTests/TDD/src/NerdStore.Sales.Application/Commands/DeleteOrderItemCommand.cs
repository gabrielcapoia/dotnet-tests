using FluentValidation;
using NerdStore.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Sales.Application.Commands
{
    public class DeleteOrderItemCommand : Command
    {
        public Guid CustomerId { get; private set; }
        public Guid ProductId { get; private set; }

        public DeleteOrderItemCommand(Guid customerId, Guid produtId)
        {
            CustomerId = customerId;
            ProductId = produtId;
        }

        public override bool IsValid()
        {
            ValidationResult = new DeleteOrderItemValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class DeleteOrderItemValidation : AbstractValidator<DeleteOrderItemCommand>
    {
        public DeleteOrderItemValidation()
        {
            RuleFor(c => c.CustomerId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do cliente inválido");

            RuleFor(c => c.ProductId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do produto inválido");
        }
    }
}
