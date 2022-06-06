using FluentValidation;
using NerdStore.Core.Messages;
using NerdStore.Sales.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Sales.Application.Commands
{
    public class InsertOrderItemCommand : Command
    {
        public InsertOrderItemCommand(Guid customerId, Guid productId, string productName, int quantity, decimal unitValue)
        {
            CustomerId = customerId;
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            UnitValue = unitValue;
        }

        public Guid CustomerId { get; }
        public Guid ProductId { get; }
        public string ProductName { get; }
        public int Quantity { get; }

        public override bool IsValid()
        {
            ValidationResult = new InsertOrderItemValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public decimal UnitValue { get; }
    }

    public class InsertOrderItemValidation : AbstractValidator<InsertOrderItemCommand>
    {
        public static string CustomerIdErrorMsg => "Invalid customer id";
        public static string ProdutIdErrorMsg => "Invalid product id";
        public static string MsgErrorName => "The product name was not entered";
         public static string QtyMaxErrorMsg => $"The maximum quantity of an item is {Order.MAX_ITEM_UNITS}";
        public static string QtyMinErrorMsg => "The minimum quantity of an item is 1";
        public static string MsgErrorValue => "The item's value must be greater than 0";

        public InsertOrderItemValidation()
        {
            RuleFor(c => c.CustomerId)
                .NotEqual(Guid.Empty)
                .WithMessage(CustomerIdErrorMsg);

            RuleFor(c => c.ProductId)
                .NotEqual(Guid.Empty)
                .WithMessage(ProdutIdErrorMsg);

            RuleFor(c => c.ProductName)
                .NotEmpty()
                .WithMessage(MsgErrorName);

            RuleFor(c => c.Quantity)
                .GreaterThan(0)
                .WithMessage(QtyMinErrorMsg)
                .LessThanOrEqualTo(Order.MAX_ITEM_UNITS)
                .WithMessage(QtyMaxErrorMsg);

            RuleFor(c => c.UnitValue)
                .GreaterThan(0)
                .WithMessage(MsgErrorValue);
        }
    }
}
