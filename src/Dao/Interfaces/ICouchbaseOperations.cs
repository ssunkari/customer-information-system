using System.Collections.Generic;
using System.Threading.Tasks;
using Couchbase;
using Domain.Models;

namespace Dao.Interfaces
{
    public interface ICouchbaseOperations
    {
        Task Upsert(Document<object> model);
        Task<IOperationResult<object>> Get(string key);
        Task<List<Customer>> GetAll();
    }
}