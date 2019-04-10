using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;
using OneOf;
using OneOf.Types;

namespace Api.Services
{
    public interface IApplicationDirector
    {
        Task<OneOf<Success, Error>> Create(Customer customer);
        Task<OneOf<Success, Error>> Update(Customer customer);
        Task<OneOf<Customer,None>> Get(string id);
        Task<OneOf<List<Customer>,None>> GetAll();
    }
}
