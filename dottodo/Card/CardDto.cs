using dottodo.Task;

namespace dottodo.Card
{
    public class CardDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";

        public int BoardId { get; set; }

        public DateTime CreatedAt { get; set; }

        public int Order { get; set; }

        public IEnumerable<TaskDto> Tasks { get; set; }
    }
}
