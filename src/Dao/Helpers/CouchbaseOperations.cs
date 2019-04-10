using System.Threading.Tasks;
using Couchbase;
using Couchbase.Core;
using Dao.Interfaces;

namespace Dao.Helpers
{
    public class CouchbaseOperations : ICouchbaseOperations
    {
        private readonly string _bucketName;

        public CouchbaseOperations(string bucketName)
        {
            _bucketName = bucketName;
        }

        private Task<IBucket> GetDefaultBucket()
        {
            return ClusterHelper.GetBucketAsync(_bucketName);
        }

        public async Task Upsert(Document<dynamic> model)
        {
            var bucket = await GetDefaultBucket();
            var documentResult = await bucket.UpsertAsync(model);
            documentResult.EnsureSuccess();
        }
        public async Task<IOperationResult<dynamic>> Get(string key)
        {
            var bucket = await GetDefaultBucket();
            var operationResult = await bucket.GetAsync<dynamic>(key);
            return operationResult;
        }
    }
}