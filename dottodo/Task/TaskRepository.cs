using Microsoft.EntityFrameworkCore;

namespace dottodo.Task
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _db;

        public TaskRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool ExistsByIdAndUserId(int id, string userId)
        {
            return _db.Tasks.Any(task => task.Id == id && task.Card.Board.UserId == userId);
        }

        public TaskEntity? GetById(int id)
        {
            return _db.Tasks.Include(task => task.Card)
                .ThenInclude(card => card.Board)
                .FirstOrDefault(card => card.Id == id);
        }

        public void RemoveById(int id)
        {
            var task = new TaskEntity { Id = id };
            _db.Tasks.Attach(task);
            _db.Tasks.Remove(task);
            _db.SaveChanges();
        }

        public TaskEntity Save(TaskEntity task)
        {
            _db.Tasks.Add(task);
            _db.SaveChanges();
            return task;
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }
    }
}
