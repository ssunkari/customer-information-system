using System.Collections.Generic;
using System.Threading.Tasks;
using Dao;
using Dao.Interfaces;
using Domain.Models;
using OneOf;
using OneOf.Types;

namespace Api.Services
{
    public class ApplicationDirector : IApplicationDirector
    {
        private readonly ICustomerRepository _customerRepository;

        public ApplicationDirector(ICustomerRepository customerRepository)
        {
            this._customerRepository = customerRepository;
        }

        public async Task<OneOf<Success, Error>> Create(Customer customer)
        {
           return await _customerRepository.Create(customer);
        }

        public async Task<OneOf<Success, Error>> Update(Customer customer)
        {
            return await _customerRepository.Update(customer);
        }

        public async Task<OneOf<Customer, None>> Get(string id)
        {
            return await _customerRepository.Get(id);
        }

        public async Task<OneOf<List<Customer>, None>> GetAll()
        {
            return await _customerRepository.GetAll();
        }
    }
}