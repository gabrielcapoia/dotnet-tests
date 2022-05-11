using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Bogus.DataSets;
using Features.Customers;
using Moq.AutoMock;
using Xunit;

namespace Features.Tests
{
    [CollectionDefinition(nameof(CustomerAutoMockerCollection))]
    public class CustomerAutoMockerCollection : ICollectionFixture<CustomerTestsAutoMockerFixture>
    {
    }

    public class CustomerTestsAutoMockerFixture : IDisposable
    {
        public CustomerService CustomerService;
        public AutoMocker Mocker;

        public Customer GenerateValidCustomer()
        {
            return GenerateCustomer(1, true).FirstOrDefault();
        }

        public IEnumerable<Customer> GetRandomCustomers()
        {
            var customers = new List<Customer>();

            customers.AddRange(GenerateCustomer(50, true).ToList());
            customers.AddRange(GenerateCustomer(50, false).ToList());

            return customers;
        }

        public IEnumerable<Customer> GenerateCustomer(int amount, bool active)
        {
            var gender = new Faker().PickRandom<Name.Gender>();

            var customer = new Faker<Customer>("pt_BR")
                .CustomInstantiator(f => new Customer(
                    Guid.NewGuid(),
                    f.Name.FirstName(gender),
                    f.Name.LastName(gender),
                    f.Date.Past(80, DateTime.Now.AddYears(-18)),
                    "",
                    active,
                    DateTime.Now))
                .RuleFor(c => c.Email, (f, c) =>
                      f.Internet.Email(c.Name.ToLower(), c.Lastname.ToLower()));

            return customer.Generate(amount);
        }

        public Customer GenerateInvalidCustomer()
        {
            var gender = new Faker().PickRandom<Name.Gender>();

            var customer = new Faker<Customer>("pt_BR")
                .CustomInstantiator(f => new Customer(
                    Guid.NewGuid(),
                    f.Name.FirstName(gender),
                    f.Name.LastName(gender),
                    f.Date.Past(1, DateTime.Now.AddYears(1)),
                    "",
                    false,
                    DateTime.Now));

            return customer;
        }

        public CustomerService GetCustomerService()
        {
            Mocker = new AutoMocker();
            CustomerService = Mocker.CreateInstance<CustomerService>();

            return CustomerService;
        }

        public void Dispose()
        {
        }
    }
}