using dottodo.Board;
using dottodo.Common;
using dottodo.Task;

namespace dottodo.Card
{
    public class CardEntity : BaseEntity
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";

        public int Order { get; set; }

        public int BoardId { get; set; }

        public BoardEntity Board { get; set; }

        public ICollection<TaskEntity> Tasks { get; } = new List<TaskEntity>();
    }
}
