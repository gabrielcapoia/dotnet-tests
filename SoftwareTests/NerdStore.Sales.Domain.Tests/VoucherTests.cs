using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.Sales.Domain.Tests
{
    public class VoucherTests
    {
        [Fact(DisplayName = "Validate Valid Type Value Voucher")]
        [Trait("Category", "Sales - Voucher")]
        public void Voucher_ValidateTypeValueVoucher_ShouldBeValid()
        {
            //Arrenge
            var voucher = new Voucher("PROMO-15", 15, null, 1, 
                DateTime.Now.AddDays(15), true, false, DiscountTypeVoucher.Value);

            //Act
            var result = voucher.ValidateIfItIsApplicable();

            //Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Validate Invalid Type Value Voucher")]
        [Trait("Category", "Sales - Voucher")]
        public void Voucher_ValidateTypeValueVoucher_ShouldBeInvalid()
        {
            //Arrenge
            var voucher = new Voucher("", null, null, 0,
                DateTime.Now.AddDays(-1), false, true, DiscountTypeVoucher.Value);

            //Act
            var result = voucher.ValidateIfItIsApplicable();

            //Assert
            Assert.False(result.IsValid);
            Assert.Equal(6, result.Errors.Count);
            Assert.Contains(VoucherApplicableValidation.ActiveErrorMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherApplicableValidation.ValidityErrorMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherApplicableValidation.MsgDiscountValue, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherApplicableValidation.MsgErrorCode, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherApplicableValidation.QuantityErrorMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherApplicableValidation.UsedErrorMsg, result.Errors.Select(c => c.ErrorMessage));
        }

        [Fact(DisplayName = "Validate Valid Type Percentage Voucher")]
        [Trait("Category", "Sales - Voucher")]
        public void Voucher_ValidateTypePercentageVoucher_ShouldBeValid()
        {
            //Arrenge
            var voucher = new Voucher("PROMO-15", null, 10, 1,
                DateTime.Now.AddDays(15), true, false, DiscountTypeVoucher.Percentage);

            //Act
            var result = voucher.ValidateIfItIsApplicable();

            //Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Validate Invalid Type Percentage Voucher")]
        [Trait("Category", "Sales - Voucher")]
        public void Voucher_ValidateTypePercentageVoucher_ShouldBeInvalid()
        {
            //Arrenge
            var voucher = new Voucher("", null, null, 0,
                DateTime.Now.AddDays(-1), false, true, DiscountTypeVoucher.Percentage);

            //Act
            var result = voucher.ValidateIfItIsApplicable();

            //Assert
            Assert.False(result.IsValid);
            Assert.Equal(6, result.Errors.Count);
            Assert.Contains(VoucherApplicableValidation.ActiveErrorMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherApplicableValidation.ValidityErrorMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherApplicableValidation.DiscountErrorMsgPercent, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherApplicableValidation.MsgErrorCode, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherApplicableValidation.QuantityErrorMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherApplicableValidation.UsedErrorMsg, result.Errors.Select(c => c.ErrorMessage));
        }
    }
}
