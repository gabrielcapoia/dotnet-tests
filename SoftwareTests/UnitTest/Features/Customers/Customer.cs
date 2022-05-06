using System;
using Features.Core;
using FluentValidation;

namespace Features.Customers
{
    public class Customer : Entity
    {
        public string Name { get; private set; }
        public string Lastname { get; private set; }
        public DateTime BirthDate { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string Email { get; private set; }
        public bool Active { get; private set; }

        protected Customer()
        {
        }

        public Customer(Guid id, string name, string lastname, DateTime birthDate, string email, bool active,
            DateTime createdAt)
        {
            Id = id;
            Name = name;
            Lastname = lastname;
            BirthDate = birthDate;
            Email = email;
            Active = active;
            CreatedAt = createdAt;
        }

        public string FullName()
        {
            return $"{Name} {Lastname}";
        }

        public bool IsEspecial()
        {
            return CreatedAt < DateTime.Now.AddYears(-3) && Active;
        }

        public void Inactivate()
        {
            Active = false;
        }

        public override bool IsValid()
        {
            ValidationResult = new CustomerValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class CustomerValidation : AbstractValidator<Customer>
    {
        public CustomerValidation()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Please, make sure you have entered the name")
                .Length(2, 150).WithMessage("The Name must be between 2 and 150 characters");

            RuleFor(c => c.Lastname)
                .NotEmpty().WithMessage("Please, make sure you have entered the lastname")
                .Length(2, 150).WithMessage("The Lastname must be between 2 and 150 characters");

            RuleFor(c => c.BirthDate)
                .NotEmpty()
                .Must(HaveMinimumAge)
                .WithMessage("Customer must be 18 years or older");

            RuleFor(c => c.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty);
        }

        public static bool HaveMinimumAge(DateTime birthDate)
        {
            return birthDate <= DateTime.Now.AddYears(-18);
        }
    }
}