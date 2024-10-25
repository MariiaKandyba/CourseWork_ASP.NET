using DTOs.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserServiceApi.Data;
using UserServiceApi.Models;

namespace UserServiceApi.Services
{
    public class UserService : IUserService
    {
        private readonly UserDbContext _dbContext;
        private readonly IConfiguration _configuration;


        public UserService(UserDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<AuthResultDto> RegisterAsync(RegisterDto registerDto)
        {
            // Перевірка наявності користувача з таким же email
            if (await _dbContext.Users.AnyAsync(u => u.Email == registerDto.Email))
                return new AuthResultDto { IsSuccess = false, ErrorMessage = "User already exists." };

            // Хешування пароля
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            // Створення нового користувача
            var user = new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                PasswordHash = hashedPassword,
                Roles = new List<Role>() 
            };

            var defaultRole = await _dbContext.Roles.SingleOrDefaultAsync(r => r.Name == "User"); 

            if (defaultRole != null)
            {
                user.Roles.Add(defaultRole);
            }

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            var token = GenerateJwtToken(user);
            return new AuthResultDto { IsSuccess = true, Token = token };
        }



        public async Task<AuthResultDto> UpdateProfileAsync(int userId, UpdateDto updateDto)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
                return new AuthResultDto { IsSuccess = false, ErrorMessage = "User not found." };

            user.FirstName = updateDto.FirstName;
            user.LastName = updateDto.LastName;
            user.Email = updateDto.Email;

            await _dbContext.SaveChangesAsync();
            return new AuthResultDto { IsSuccess = true };
        }


        public async Task<AuthResultDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                return new AuthResultDto { IsSuccess = false, ErrorMessage = "Invalid credentials." };

            var token = GenerateJwtToken(user);
            return new AuthResultDto { IsSuccess = true, Token = token };
        }

        private string GenerateJwtToken(User user)
        {
            var roleClaims = user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name)).ToArray();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName),
            };

            claims.AddRange(roleClaims);


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(5000),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }

}
