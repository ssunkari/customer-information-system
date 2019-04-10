using System;
using System.Collections.Generic;
using Couchbase;
using Couchbase.Authentication;
using Couchbase.Configuration.Client;

namespace Api.Couchbase
{
    public  class CouchbaseStartup : ICouchbaseStartup
    {
        public CouchbaseStartup(CouchbaseConfiguration configuration)
        {
            Register(configuration);
        }
        private static void Register(CouchbaseConfiguration configuration)
        {
            ClusterHelper.Initialize(new ClientConfiguration
            {
                Servers = new List<Uri> { new Uri(configuration.Host) }
            });
            var username = configuration.Username;
            var password = configuration.Password;

            // provide authentication to cluster
            ClusterHelper.Get().Authenticate(new PasswordAuthenticator(username, password));
        }
    }
}