using Couchbase;
using Dao.Interfaces;
using Domain.Models;

namespace Dao.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ICouchbaseOperations _couchbaseOperations;

        public CustomerRepository(ICouchbaseOperations couchbaseOperations)
        {
            _couchbaseOperations = couchbaseOperations;
        }

        public void Create(Customer customer)
        {
            var document = new Document<dynamic>
                {
                    Id = customer.Email.GetHashCode().ToString(),
                    Content = customer
                };
            _couchbaseOperations.Upsert(document);
        }
    }
}
