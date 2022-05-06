using System.Collections.Generic;
using System.Linq;
using MediatR;

namespace Features.Customers
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMediator _mediator;

        public CustomerService(ICustomerRepository customerRepository, 
                              IMediator mediator)
        {
            _customerRepository = customerRepository;
            _mediator = mediator;
        }

        public IEnumerable<Customer> GetAllActives()
        {
            return _customerRepository.GetAll().Where(c => c.Active);
        }

        public void Add(Customer customer)
        {
            if (!customer.IsValid())
                return;

            _customerRepository.Add(customer);
            _mediator.Publish(new CustomerEmailNotification("admin@me.com", customer.Email, "Hello", "Welcome!"));
        }

        public void Update(Customer customer)
        {
            if (!customer.IsValid())
                return;

            _customerRepository.Update(customer);
            _mediator.Publish(new CustomerEmailNotification("admin@me.com", customer.Email, "Changes", "Take a look!"));
        }

        public void Inactivate(Customer customer)
        {
            if (!customer.IsValid())
                return;

            customer.Inactivate();
            _customerRepository.Update(customer);
            _mediator.Publish(new CustomerEmailNotification("admin@me.com", customer.Email, "See you soon", "See you later!"));
        }

        public void Remove(Customer customer)
        {
            _customerRepository.Remove(customer.Id);
            _mediator.Publish(new CustomerEmailNotification("admin@me.com", customer.Email, "Good bye", "Have a good journey!"));
        }

        public void Dispose()
        {
            _customerRepository.Dispose();
        }
    }
}