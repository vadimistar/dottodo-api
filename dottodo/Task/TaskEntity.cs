using dottodo.Card;
using dottodo.Common;

namespace dottodo.Task
{
    public class TaskEntity : BaseEntity
    {
        public int Id { get; set; }

        public string? Title { get; set; } = "";

        public int Order { get; set; }

        public int CardId { get; set; }

        public CardEntity Card { get; set; }
    }
}
