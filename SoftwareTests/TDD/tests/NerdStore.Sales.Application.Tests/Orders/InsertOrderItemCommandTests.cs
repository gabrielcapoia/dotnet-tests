using NerdStore.Sales.Application.Commands;
using NerdStore.Sales.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.Sales.Application.Tests.Orders
{
    public class InsertOrderItemCommandTests
    {
        [Fact(DisplayName = "Insert Valid Item Command")]
        [Trait("Category", "Sales - Order Commands")]
        public void InsertOrderItemCommand_ValidCommand_ShouldValidateWithSuccess()
        {
            // Arrange
            var orderCommand = new InsertOrderItemCommand(Guid.NewGuid(), 
                Guid.NewGuid(), "Product Test", 2, 100);

            // Act
            var result = orderCommand.IsValid();

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Insert Invalid Item Command")]
        [Trait("Category", "Sales - Order Commands")]
        public void InsertOrderItemCommand_InvalidCommand_ShouldValidateWithErrors()
        {
            // Arrange
            var orderCommand = new InsertOrderItemCommand(Guid.Empty,
                Guid.Empty, string.Empty, 0, 0);

            // Act
            var result = orderCommand.IsValid();

            // Assert
            Assert.False(result);
            Assert.Equal(5, orderCommand.ValidationResult.Errors.Count);
            Assert.Contains(InsertOrderItemValidation.CustomerIdErrorMsg, orderCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(InsertOrderItemValidation.ProdutIdErrorMsg, orderCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(InsertOrderItemValidation.MsgErrorName, orderCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(InsertOrderItemValidation.QtyMinErrorMsg, orderCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(InsertOrderItemValidation.MsgErrorValue, orderCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            
        }

        [Fact(DisplayName = "Insert Item Command With Quantity Greater Than Allowed")]
        [Trait("Category", "Sales - Order Commands")]
        public void InsertOrderItemCommand_QuantityGreaterThanAllowed_ShouldValidateWithErrors()
        {
            // Arrange
            var orderCommand = new InsertOrderItemCommand(Guid.NewGuid(),
                Guid.NewGuid(), "Product Test", Order.MAX_ITEM_UNITS + 1, 100);

            // Act
            var result = orderCommand.IsValid();

            // Assert
            Assert.False(result);                        
            Assert.Contains(InsertOrderItemValidation.QtyMaxErrorMsg, orderCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));

        }
    }
}
