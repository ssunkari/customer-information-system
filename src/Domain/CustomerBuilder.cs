using Domain.Models;

namespace Domain
{
    public class CustomerBuilder
    {
        private string _firstname;
        private string _surname;
        private string _email;
        private string hashPassword;

        public static CustomerBuilder Create()
        {
            return new CustomerBuilder();
        }
        public CustomerBuilder WithFirstName(string value)
        {
            _firstname = value;
            return this;
        }
        public CustomerBuilder WithSurname(string value)
        {
            _surname = value;
            return this;
        }

        public CustomerBuilder WithEmail(string value)
        {
            _email = value;
            return this;
        }
        public CustomerBuilder WithPassword(string value)
        {
            hashPassword = value?.GetHashCode().ToString();
            return this;
        }

        public Customer Build()
        {
            return new Customer(_firstname, _surname, _email, hashPassword);
        }
    }
}