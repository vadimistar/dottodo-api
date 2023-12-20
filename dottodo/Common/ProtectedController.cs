using dottodo.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dottodo.Common
{
    [Authorize]
    public class ProtectedController : ControllerBase
    {
        protected readonly IUserRepository _userRepository;

        public ProtectedController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        protected UserEntity? GetCurrentUser()
        {
            return _userRepository.GetByUsername(User.Identity?.Name ?? "");
        }
    }
}
