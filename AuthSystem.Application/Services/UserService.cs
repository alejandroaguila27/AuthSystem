using AuthSystem.Domain.Entities;
using AuthSystem.Domain.Interfaces;
using Microsoft.VisualStudio.Services.WebApi.Jwt;
using System;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace AuthSystem.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;

        public UserService(IUserRepository userRepository, IPasswordService passwordService, IJwtService iJwtService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _jwtService = iJwtService; // Inyecta el servicio JwtService
        }

        public async Task<User> CreateUser(string username, string email, string password)
        {
            // Verificar si el usuario ya existe
            var existingUser = await _userRepository.GetByEmailAsync(email);
            if (existingUser != null)
            {
                // Puedes lanzar una excepción, devolver un resultado personalizado, etc.
                throw new InvalidOperationException("El usuario con este correo electrónico ya existe.");
            }

            // Crear una nueva instancia de User y establecer propiedades
            var newUser = new User
            {
                Username = username,
                Email = email,
                PasswordHash = _passwordService.HashPassword(password)
            };

            // Guardar el usuario en la base de datos
            await _userRepository.AddAsync(newUser);

            return newUser;
        }

        public async Task<string> AuthenticateUser(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);

            if (user == null || !_passwordService.VerifyPassword(password, user.PasswordHash))
            {
                // Puedes lanzar una excepción personalizada
                throw new InvalidCredentialsException("Credenciales no válidas");
            }

            // Autenticación exitosa, generar y devolver el token JWT
            var token = _jwtService.GenerateToken(user.Id);
            return token;
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

    }
}
