using System.Threading.Tasks;
using Domain.Models;
using OneOf;
using OneOf.Types;

namespace Dao.Interfaces
{
    public interface ICustomerRepository
    {
        Task<OneOf<Success, Error>> Create(Customer customer);
        Task<OneOf<Success, Error>> Update(Customer customer);
        Task<OneOf<Customer, None>> Get(string id);
    }
}