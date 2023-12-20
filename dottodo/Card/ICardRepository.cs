namespace dottodo.Card
{
    public interface ICardRepository
    {
        CardEntity Save(CardEntity card);

        IEnumerable<CardEntity> GetCardsByUserIdAndBoardId(string userId, int boardId);

        CardEntity? GetById(int id);

        bool ExistsByIdAndUserId(int id, string userId);

        void RemoveById(int id);

        void SaveChanges();
    }
}
