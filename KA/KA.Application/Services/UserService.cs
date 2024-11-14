using AutoMapper;
using KA.Application.DTO;
using KA.Application.Interfaces;
using KA.Domain.Entities;
using KA.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace KA.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticatedService _authenticatedService;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        public UserService(IAuthenticatedService authenticatedService, IUserRepository userRepository, ILogger<UserService> logger)
        {
            _authenticatedService = authenticatedService;
            _userRepository = userRepository;
            _logger = logger;

            // This configuration sets up a bidirectional mapping between the User and UserDTO classes.
            _mapper = new MapperConfiguration(delegate (IMapperConfigurationExpression cfg)
            {
                cfg.CreateMap<User, UserDTO>().ReverseMap();
            }).CreateMapper();
        }

        /// <summary>
        /// Validate if User exixst in DB and the username and Password are correct
        /// </summary>
        /// <param name="username">usename</param>
        /// <param name="password">password</param>
        /// <returns>User if valide</returns>
        public async Task<UserDTO?> ValidateUserAsync(string username, string password)
        {
            var user = await _userRepository.GetUserAsync(username);

            if (user != null && !_authenticatedService.ValidatePassword(password, user.Password))
            {
                _logger.LogError($"No user found or validated!");
                return null;
            }

            return _mapper.Map<UserDTO>(user);
        }
    }
}
