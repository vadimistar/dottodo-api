using dottodo.Card;
using dottodo.Common;
using dottodo.User;
using Microsoft.AspNetCore.Mvc;

namespace dottodo.Board
{
    [Route("api/board")]
    [ApiController]
    public class BoardController : ProtectedController
    {
        private readonly IBoardRepository _boardRepository;
        private readonly IBoardDtoMapper _boardMapper;

        public BoardController(IUserRepository userRepository, IBoardRepository boardRepository, IBoardDtoMapper boardMapper)
            : base(userRepository)
        {
            _boardRepository = boardRepository;
            _boardMapper = boardMapper;
        }

        [HttpGet]
        public IActionResult FetchAllBoards()
        {
            var user = GetCurrentUser();

            if (user == null)
            {
                return Unauthorized();
            }

            return Ok(_boardRepository.GetAllByUserId(user.Id)
                .Select(_boardMapper.Map)
                .OrderByDescending(board => board.UpdatedAt)
                .ToList());
        }
        
        [HttpGet("{id}")]
        public IActionResult FetchById(int id)
        {
            var user = GetCurrentUser();

            if (user == null)
            {
                return Unauthorized();
            }

            if (!_boardRepository.ExistsByIdAndUserId(id, user.Id))
            {
                return BadRequest("invalid id");
            }
            
            var board = _boardRepository.GetById(id);

            if (board == null)
            {
                return BadRequest("invalid id");
            }

            return Ok(_boardMapper.Map(board));
        }

        [HttpPost]
        public IActionResult CreateBoard(string title)
        {
            var user = GetCurrentUser();

            if (user == null)
            {
                return Unauthorized();
            }

            var board = new BoardEntity
            {
                Title = title,
                UserId = user.Id,
            };

            board = _boardRepository.Save(board);

            return Ok(_boardMapper.Map(board));
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateBoard(int id, string title)
        {
            var user = GetCurrentUser();    

            if (user == null) { return Unauthorized(); }

            var board = _boardRepository.GetById(id);

            if (board == null || board.UserId != user.Id)
            {
                return BadRequest("invalid id"); 
            }

            board.Title = title;

            _boardRepository.SaveChanges();

            return Ok(_boardMapper.Map(board));
        }

        [HttpDelete("{id}")]
        public IActionResult RemoveBoard(int id)
        {
            var user = GetCurrentUser();

            if (user == null) { return Unauthorized(); }

            if (!_boardRepository.ExistsByIdAndUserId(id, user.Id))
            {
                return BadRequest("invalid id");
            }

            _boardRepository.RemoveById(id);

            return Ok();
        }
    }
}
