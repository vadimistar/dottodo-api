using dottodo.Task;

namespace dottodo.Card
{
    public interface ICardDtoMapper
    {
        CardDto Map(CardEntity card, ITaskDtoMapper taskDtoMapper);
    }
}
