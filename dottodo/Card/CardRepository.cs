
using Microsoft.EntityFrameworkCore;

namespace dottodo.Card
{
    public class CardRepository : ICardRepository
    {
        private readonly ApplicationDbContext _db;

        public CardRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public CardEntity Save(CardEntity card)
        {
            _db.Cards.Add(card);     
            _db.SaveChanges();
            return card;
        }

        public IEnumerable<CardEntity> GetCardsByUserIdAndBoardId(string userId, int boardId)
        {
            return _db.Cards.Include(card => card.Tasks)
                .Where(card => card.BoardId == boardId && card.Board.UserId == userId);
        }

        public CardEntity? GetById(int id)
        {
            return _db.Cards.Include(c => c.Board).FirstOrDefault(card => card.Id == id);
        }

        public bool ExistsByIdAndUserId(int id, string userId)
        {
            return _db.Cards.Any(card => card.Id == id && card.Board.UserId == userId);
        }

        public void RemoveById(int id)
        {
            var card = new CardEntity { Id = id };
            _db.Cards.Attach(card);
            _db.Cards.Remove(card);
            _db.SaveChanges();
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }
    }
}
