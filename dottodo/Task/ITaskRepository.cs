namespace dottodo.Task
{
    public interface ITaskRepository
    {
        TaskEntity Save(TaskEntity task);

        TaskEntity? GetById(int id);

        bool ExistsByIdAndUserId(int id, string userId);

        void RemoveById(int id);

        void SaveChanges();
    }
}
