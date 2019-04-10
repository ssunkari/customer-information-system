using Dao;
using Dao.Interfaces;
using Domain.Models;

namespace Api.Services
{
    public class ApplicationDirector : IApplicationDirector
    {
        private readonly ICustomerRepository _customerRepository;

        public ApplicationDirector(ICustomerRepository customerRepository)
        {
            this._customerRepository = customerRepository;
        }

        public void Create(Customer customer)
        {
            _customerRepository.Create(customer);
        }
    }
}