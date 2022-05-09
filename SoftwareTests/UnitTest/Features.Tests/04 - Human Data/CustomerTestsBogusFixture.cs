using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Bogus.DataSets;
using Features.Customers;
using Xunit;

namespace Features.Tests
{
    [CollectionDefinition(nameof(CustomerBogusCollection))]
    public class CustomerBogusCollection : ICollectionFixture<CustomerTestsBogusFixture>
    {}

    public class CustomerTestsBogusFixture : IDisposable
    {
        public Customer GenerateValidCustomer()
        {
            return GenerateCustomers(1, true).FirstOrDefault();
        }

        public IEnumerable<Customer> GetRandomCustomers()
        {
            var customers = new List<Customer>();

            customers.AddRange(GenerateCustomers(50, true).ToList());
            customers.AddRange(GenerateCustomers(50, false).ToList());

            return customers;
        }

        public IEnumerable<Customer> GenerateCustomers(int quantidade, bool ativo)
        {
            var gender = new Faker().PickRandom<Name.Gender>();

            //var email = new Faker().Internet.Email("eduardo","pires","gmail");
            //var clientefaker = new Faker<Cliente>();
            //clientefaker.RuleFor(c => c.Nome, (f, c) => f.Name.FirstName());

            var customer = new Faker<Customer>("pt_BR")
                .CustomInstantiator(f => new Customer(
                    Guid.NewGuid(), 
                    f.Name.FirstName(gender),
                    f.Name.LastName(gender),
                    f.Date.Past(80,DateTime.Now.AddYears(-18)),
                    "",
                    ativo,
                    DateTime.Now))
                .RuleFor(c=>c.Email, (f,c) => 
                    f.Internet.Email(c.Name.ToLower(), c.Lastname.ToLower()));

            return customer.Generate(quantidade);
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

        public void Dispose()
        {
        }
    }
}