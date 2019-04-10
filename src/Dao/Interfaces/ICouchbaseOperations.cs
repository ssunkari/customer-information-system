using System.Threading.Tasks;
using Couchbase;

namespace Dao.Interfaces
{
    public interface ICouchbaseOperations
    {
        Task Upsert(Document<object> model);
        Task<IOperationResult<object>> Get(string key);
    }
}