using Domain.Models;

namespace Dao.Interfaces
{
    public interface ICustomerRepository
    {
        void Create(Customer customer);
    }
}