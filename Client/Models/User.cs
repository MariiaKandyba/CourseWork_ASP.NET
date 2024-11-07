namespace Client.Models
{
    public class User
    {
        public string Id { get; set; } 
        public string Email { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public string Role { get; set; } 
        public bool IsAuthenticated { get; set; } 

        public User()
        {
        }

        public User(string id, string email, string firstName, string lastName, string role, bool isAuthenticated)
        {
            Id = id;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Role = role;
            IsAuthenticated = isAuthenticated;
        }

    }

}
