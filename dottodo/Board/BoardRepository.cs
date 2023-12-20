using Microsoft.EntityFrameworkCore;

namespace dottodo.Board
{
    public class BoardRepository : IBoardRepository
    {
        private readonly ApplicationDbContext _db;

        public BoardRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public BoardEntity Save(BoardEntity board)
        {
            _db.Boards.Add(board);
            _db.SaveChanges();
            return board;
        }

        public IEnumerable<BoardEntity> GetAllByUserId(string userId)
        {
            return _db.Boards.Where(b => b.UserId == userId).Include(board => board.Cards);
        }

        public BoardEntity? GetById(int id)
        {
            return _db.Boards.Include(board => board.Cards).FirstOrDefault(board => board.Id == id);
        }

        public bool ExistsByIdAndUserId(int id, string userId)
        {
            return _db.Boards.Any(b => b.Id == id && b.UserId == userId);
        }

        public void RemoveById(int id)
        {
            var board = new BoardEntity { Id = id };
            _db.Boards.Attach(board);
            _db.Boards.Remove(board);
            _db.SaveChanges();
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }
    }
}
