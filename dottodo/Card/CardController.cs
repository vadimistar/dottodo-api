using dottodo.Board;
using dottodo.Common;
using dottodo.Task;
using dottodo.User;
using Microsoft.AspNetCore.Mvc;

namespace dottodo.Card
{
    [Route("api/board/{boardId}/card")]
    [ApiController]
    public class CardController : ProtectedController
    {
        private readonly ICardRepository _cardRepository;
        private readonly ICardDtoMapper _cardDtoMapper;
        private readonly ITaskDtoMapper _taskDtoMapper;
        private readonly IBoardRepository _boardRepository;

        public CardController(IUserRepository userRepository, ICardRepository cardRepository, ICardDtoMapper cardDtoMapper, ITaskDtoMapper taskDtoMapper, IBoardRepository boardRepository)
            : base(userRepository)
        {
            _cardRepository = cardRepository;
            _cardDtoMapper = cardDtoMapper;
            _taskDtoMapper = taskDtoMapper;
            _boardRepository = boardRepository;
        }

        [HttpGet]
        public IActionResult FetchCards(int boardId)
        {
            var user = GetCurrentUser();

            if (user == null)
            {
                return Unauthorized();
            }

            var cards = _cardRepository.GetCardsByUserIdAndBoardId(user.Id, boardId);

            return Ok(cards
                .Select(card => _cardDtoMapper.Map(card, _taskDtoMapper))
                .OrderBy(card => card.CreatedAt));
        }

        [HttpPost]
        public IActionResult AddCard(int boardId, string title)
        {
            var user = GetCurrentUser();

            if (user == null) { return Unauthorized(); }

            var card = new CardEntity
            {
                Title = title,
                BoardId = boardId,
            };

            card = _cardRepository.Save(card);

            return Ok(_cardDtoMapper.Map(card, _taskDtoMapper));
        }

        [HttpPatch("~/api/board/card/{id}")]
        public IActionResult UpdateCard(int id, string title)
        {
            var user = GetCurrentUser();

            if (user == null) { return Unauthorized(); }
            
            var card = _cardRepository.GetById(id);

            if (card == null || card.Board.UserId != user.Id)
            {
                return BadRequest("invalid id");
            }
            
            card.Title = title;
            _cardRepository.SaveChanges();

            return Ok(_cardDtoMapper.Map(card, _taskDtoMapper));
        }

        [HttpDelete("~/api/board/card/{id}")]
        public IActionResult RemoveCard(int id)
        {
            var user = GetCurrentUser();

            if (user == null) { return Unauthorized(); }

            if (!_cardRepository.ExistsByIdAndUserId(id, user.Id))
            {
                return BadRequest("invalid id");
            }

            _cardRepository.RemoveById(id);

            return Ok();
        }
    }
}
