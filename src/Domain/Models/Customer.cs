namespace Domain.Models
{
    public class Customer
    {
        public string FirstName { get; }
        public string Surname { get; }
        public string Email { get; }
        public string Password { get; }

        public Customer(string firstName, string surname, string email, string password)
        {
            FirstName = firstName;
            Surname = surname;
            Email = email;
            Password = password;
        }
    }
}
