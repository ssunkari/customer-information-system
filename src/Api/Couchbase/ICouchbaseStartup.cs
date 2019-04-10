namespace Api.Couchbase
{
    public interface ICouchbaseStartup
    {
        void Register(CouchbaseConfiguration configuration);
    }
}