namespace dottodo.Board
{
    public interface IBoardRepository
    {
        BoardEntity Save(BoardEntity board);

        IEnumerable<BoardEntity> GetAllByUserId(string userId);

        BoardEntity? GetById(int id);

        bool ExistsByIdAndUserId(int id, string userId);

        void RemoveById(int id);

        void SaveChanges();
    }
}
