namespace dottodo.Task
{
    public class TaskDto
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public int CardId { get; set; }

        public int Order { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
