using FluentValidation;
using FluentValidation.Results;
using NerdStore.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Sales.Domain
{
    public class Voucher : Entity
    {
        public string Code { get; private set; }
        public decimal? DiscountAmount { get; private set; }
        public decimal? DiscountPercentage { get; private set; }
        public int Quantity { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public bool Active { get; private set; }
        public bool Used { get; private set; }
        public DiscountTypeVoucher DiscountTypeVoucher { get; private set; }

        public ICollection<Order> Orders { get; set; }

        public Voucher(string code, decimal? discountAmount, decimal? discountPercentage, int quantity, 
            DateTime expirationDate, bool active, bool used, DiscountTypeVoucher discountTypeVoucher)
        {
            Code = code;
            DiscountAmount = discountAmount;
            DiscountPercentage = discountPercentage;
            Quantity = quantity;
            ExpirationDate = expirationDate;
            Active = active;
            Used = used;
            DiscountTypeVoucher = discountTypeVoucher;
        }

        public ValidationResult ValidateIfItIsApplicable()
        {
            return new VoucherApplicableValidation().Validate(this);
        }
    }

    public class VoucherApplicableValidation : AbstractValidator<Voucher>
    {
        public static string MsgErrorCode => "Voucher not valid code.";
        public static string ValidityErrorMsg => "This voucher is expired.";
        public static string ActiveErrorMsg => "This voucher is no longer valid.";
        public static string UsedErrorMsg => "This voucher has already been used.";
        public static string QuantityErrorMsg => "This voucher is no longer available";
        public static string MsgDiscountValue => "Discount value must be greater than 0";
        public static string DiscountErrorMsgPercent => "Discount percentage value must be greater than 0";

        public VoucherApplicableValidation()
        {
            RuleFor(c => c.Code)
                .NotEmpty()
                .WithMessage(MsgErrorCode);

            RuleFor(c => c.ExpirationDate)
                .Must(ExpirationDateUpperThanCurrent)
                .WithMessage(ValidityErrorMsg);

            RuleFor(c => c.Active)
                .Equal(true)
                .WithMessage(ActiveErrorMsg);

            RuleFor(c => c.Used)
                .Equal(false)
                .WithMessage(UsedErrorMsg);

            RuleFor(c => c.Quantity)
                .GreaterThan(0)
                .WithMessage(QuantityErrorMsg);

            When(f => f.DiscountTypeVoucher == DiscountTypeVoucher.Value, () =>
            {
                RuleFor(f => f.DiscountAmount)
                    .NotNull()
                    .WithMessage(MsgDiscountValue)
                    .GreaterThan(0)
                    .WithMessage(MsgDiscountValue);
            });

            When(f => f.DiscountTypeVoucher == DiscountTypeVoucher.Percentage, () =>
            {
                RuleFor(f => f.DiscountPercentage)
                    .NotNull()
                    .WithMessage(DiscountErrorMsgPercent)
                    .GreaterThan(0)
                    .WithMessage(DiscountErrorMsgPercent);
            });            
        }

        protected static bool ExpirationDateUpperThanCurrent(DateTime expirationDate)
        {
            return expirationDate >= DateTime.Now;
        }
    }

    public enum DiscountTypeVoucher
    {
        Percentage = 0,
        Value = 1
    }
}
