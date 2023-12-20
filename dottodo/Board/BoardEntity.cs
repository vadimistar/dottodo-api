using dottodo.Card;
using dottodo.Common;
using dottodo.User;

namespace dottodo.Board
{
    public class BoardEntity : BaseEntity
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";

        public string UserId { get; set; } = "";

        public UserEntity User { get; set; }

        public ICollection<CardEntity> Cards { get; } = new List<CardEntity>();
    }
}
