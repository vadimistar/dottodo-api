using dottodo.Task;

namespace dottodo.Card
{
    public class CardDtoMapper : ICardDtoMapper
    {
        public CardDto Map(CardEntity card, ITaskDtoMapper taskDtoMapper)
        {
            return new CardDto
            {
                Id = card.Id,
                Title = card.Title,
                BoardId = card.BoardId,
                CreatedAt = card.CreatedAt,
                Order = card.Order,
                Tasks = card.Tasks.Select(taskDtoMapper.Map)
                    .OrderBy(card => card.CreatedAt),
            };
        }
    }
}
