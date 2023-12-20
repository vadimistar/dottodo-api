using dottodo.Card;
using dottodo.Common;
using dottodo.User;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace dottodo.Task
{
    [Route("api/board/card/{cardId}/task")]
    [ApiController]
    public class TaskController : ProtectedController
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ICardRepository _cardRepository;
        private readonly ITaskDtoMapper _taskDtoMapper;

        public TaskController(IUserRepository userRepository, ITaskRepository taskRepository, ICardRepository cardRepository, ITaskDtoMapper taskDtoMapper)
            : base(userRepository)
        {
            _taskRepository = taskRepository;
            _cardRepository = cardRepository;
            _taskDtoMapper = taskDtoMapper;
        }

        [HttpPost]
        public IActionResult AddTask(int cardId, string title)
        {
            var user = GetCurrentUser();

            if (user == null) { return Unauthorized(); }

            if (!_cardRepository.ExistsByIdAndUserId(cardId, user.Id))
            {
                return BadRequest("invalid id");
            }

            var task = new TaskEntity { Title = title, CardId = cardId };

            task = _taskRepository.Save(task);

            return Ok(_taskDtoMapper.Map(task));
        }

        [HttpPatch("~/api/board/card/task/{id}")]
        public IActionResult UpdateTask(int id, string? title, int? cardId)
        {
            var user = GetCurrentUser();

            if (user == null) { return Unauthorized(); }

            var task = _taskRepository.GetById(id);

            if (task == null || task.Card.Board.UserId != user.Id)
            {
                return BadRequest("invalid id");
            }
            
            if (cardId != null)
            {
                if (!_cardRepository.ExistsByIdAndUserId(cardId.Value, user.Id))
                {
                    return BadRequest("invalid id");
                }

                task.CardId = cardId.Value;
            }
            else if (!String.IsNullOrEmpty(title))
            {
                task.Title = title;
            }

            _taskRepository.SaveChanges();

            return Ok(_taskDtoMapper.Map(task));
        }

        [HttpDelete("~/api/board/card/task/{id}")]
        public IActionResult RemoveTask(int id)
        {
            var user = GetCurrentUser();

            if (user == null) { return Unauthorized(); }

            if (!_taskRepository.ExistsByIdAndUserId(id, user.Id))
            {
                return BadRequest("invalid id");
            }

            _taskRepository.RemoveById(id);

            return Ok();
        }
    }
}
