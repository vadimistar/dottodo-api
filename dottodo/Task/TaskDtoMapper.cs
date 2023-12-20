namespace dottodo.Task
{
    public class TaskDtoMapper : ITaskDtoMapper
    {
        public TaskDto Map(TaskEntity task)
        {
            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                CardId = task.CardId,
                Order = task.Order,
                CreatedAt = task.CreatedAt,
            };
        }
    }
}
