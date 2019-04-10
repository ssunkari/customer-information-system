using System.Collections.Generic;
using Api.Couchbase;
using Api.Helpers;
using Api.Services;
using Dao;
using Dao.Helpers;
using Dao.Interfaces;
using Dao.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            // configure basic authentication 
            services.AddScoped<IUserService>(provider =>
            {
                var configurationSection = Configuration.GetSection("BasicAuthUserList");
                var users = configurationSection.Get<List<User>>();
                return new UserService(users);
            });
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
            ConfigureExternalDependencies(services);
        }

        protected virtual void ConfigureExternalDependencies(IServiceCollection services)
        {
            var couchbaseConfiguration = Configuration.GetSection("couchbase").Get<CouchbaseConfiguration>();
            services.AddSingleton<ICouchbaseStartup>(p => new CouchbaseStartup());
            services.AddScoped<IApplicationDirector,ApplicationDirector>();
            services.AddScoped<ICustomerRepository,CustomerRepository>();
            services.AddScoped<ICouchbaseOperations>(p=>new CouchbaseOperations(couchbaseConfiguration.BucketName));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

           var couchbaseHandler = app.ApplicationServices.GetService<ICouchbaseStartup>();
           var couchbaseConfiguration = Configuration.GetSection("couchbase").Get<CouchbaseConfiguration>();
           couchbaseHandler.Register(couchbaseConfiguration);
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
