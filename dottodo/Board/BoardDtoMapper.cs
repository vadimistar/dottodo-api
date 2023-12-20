namespace dottodo.Board
{
    public class BoardDtoMapper : IBoardDtoMapper
    {
        public BoardDto Map(BoardEntity board)
        {
            return new BoardDto
            {
                Id = board.Id,
                Title = board.Title,
                CreatedAt = board.CreatedAt,
                UpdatedAt = board.UpdatedAt,
            };
        }
    }
}
