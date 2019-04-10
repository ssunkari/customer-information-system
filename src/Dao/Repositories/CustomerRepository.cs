using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Couchbase;
using Dao.Interfaces;
using Domain.Models;
using Newtonsoft.Json;
using OneOf;
using OneOf.Types;

namespace Dao.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ICouchbaseOperations _couchbaseOperations;

        public CustomerRepository(ICouchbaseOperations couchbaseOperations)
        {
            _couchbaseOperations = couchbaseOperations;
        }

        public async Task<OneOf<Success,Error>> Create(Customer customer)
        {
            var key = customer.Email.GetStableHashCode().ToString();
            var customerAlreadyExist = await _couchbaseOperations.Get(key);
            if (customerAlreadyExist.Success)
            {
                return new Error();
            }
            var document = new Document<dynamic>
                {
                    Id = key,
                    Content = customer
                };
            await _couchbaseOperations.Upsert(document);
            return new Success();
        }

        public async Task<OneOf<Success, Error>> Update(Customer customer)
        {
            var key = customer.Email.GetStableHashCode().ToString();
            var customerAlreadyExist = await _couchbaseOperations.Get(key);
            if (!customerAlreadyExist.Success)
            {
                return new Error();
            }
            var document = new Document<dynamic>
            {
                Id = key,
                Content = customer
            };
            await _couchbaseOperations.Upsert(document);
            return new Success();
        }

        public async Task<OneOf<Customer, None>> Get(string id)
        {
            var customerAlreadyExist = await _couchbaseOperations.Get(id);
            if (customerAlreadyExist.Success)
            {
                return JsonConvert.DeserializeObject<Customer>(customerAlreadyExist.Value.ToString());
            }
            return new None();
        }

        public async Task<OneOf<List<Customer>, None>> GetAll()
        {
            var customerAlreadyExist = await _couchbaseOperations.GetAll();
            if(customerAlreadyExist == null)
            return new None();
            return customerAlreadyExist;
        }
    }
}
