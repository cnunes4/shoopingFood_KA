using AutoMapper;
using KA.Application.DTO;
using KA.Application.Interfaces;
using KA.Domain.Entities;
using KA.Domain.Interfaces;

namespace KA.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticatedService _authenticatedService;
        private readonly IMapper _mapper;
        public UserService(IAuthenticatedService authenticatedService, IUserRepository userRepository)
        {
            _authenticatedService = authenticatedService;
            _userRepository = userRepository;

            _mapper = new MapperConfiguration(delegate (IMapperConfigurationExpression cfg)
            {
                cfg.CreateMap<User, UserDTO>().ReverseMap();
            }).CreateMapper();
        }


        public async Task<UserDTO?> ValidateUserAsync(string username, string password)
        {
            var user = await _userRepository.GetUserAsync(username);

            if (user != null && !_authenticatedService.ValidatePassword(password, user.Password))
            {
                return null;
            }

            return _mapper.Map<UserDTO>(user);
        }
    }
}
