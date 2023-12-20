using dottodo.Board;
using Microsoft.AspNetCore.Identity;

namespace dottodo.User
{
    public class UserEntity : IdentityUser
    {
        public ICollection<BoardEntity> Boards { get; } = new List<BoardEntity>();

        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpiresAt { get; set; } = DateTime.UtcNow;
    }
}
