using System;
using Features.Customers;
using Xunit;

namespace Features.Tests
{
    [CollectionDefinition(nameof(CustomerCollection))]
    public class CustomerCollection : ICollectionFixture<CustomerTestsFixture>
    {}

    public class CustomerTestsFixture : IDisposable
    {
        public Customer GenerateValidCustomer()
        {
            var customer = new Customer(
                Guid.NewGuid(),
                "Gabriel",
                "Capoia",
                DateTime.Now.AddYears(-30),
                "gabriel@capoia.com",
                true,
                DateTime.Now);

            return customer;
        }

        public Customer GenerateCustomerInvalid()
        {
            var customer = new Customer(
                Guid.NewGuid(),
                "",
                "",
                DateTime.Now,
                "a2b.com",
                true,
                DateTime.Now);

            return customer;
        }

        public void Dispose()
        {
        }
    }
}