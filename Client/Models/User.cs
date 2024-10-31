namespace Client.Models
{
    public class User
    {
        public string Id { get; set; } // Ідентифікатор користувача
        public string Email { get; set; } // Email користувача
        public string FirstName { get; set; } // Ім'я користувача
        public string LastName { get; set; } // Прізвище користувача
        public string Role { get; set; } // Роль користувача
        public bool IsAuthenticated { get; set; } // Статус аутентифікації

        // Можна додати конструктор для зручності
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

        // Можна додати методи або властивості за потребою
    }

}
