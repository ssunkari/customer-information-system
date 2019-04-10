using Domain.Models;

namespace Domain
{
    public class CustomerBuilder
    {
        private string _firstname;
        private string _surname;
        private string _email;
        private string _password;

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
            _password = value;
            return this;
        }

        public Customer Build()
        {
            return new Customer(_firstname,_surname,_email,_password);
        }
    }
}