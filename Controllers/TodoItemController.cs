using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using PracticeProject.Dtos.TodoItem;
using PracticeProject.Helpers;
using PracticeProject.Interfaces;
using PracticeProject.Models;
using PracticeProject.Service;

namespace PracticeProject.Controllers
{
    [ApiController]
    [Route("api/todoItem")]
    public class TodoItemController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITodoItemRepository _todoItemRepository;
        private readonly ITokenService _tokenService;
        public TodoItemController(IMapper mapper, ITodoItemRepository todoItemRepository, ITokenService tokenService)
        {
            _mapper = mapper;
            _todoItemRepository = todoItemRepository;
            _tokenService = tokenService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] TodoItemQueryObject query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var todoItems = await _todoItemRepository.GetAllAsync(query);
            var todoItemDtos = _mapper.Map<List<TodoItemDto>>(todoItems);
            return Ok(todoItemDtos);
        }

        // Attribute Routing
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var todoItem = await _todoItemRepository.GetByIdAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            // 轉成給User的TodoItemDto
            var todoItemDto = _mapper.Map<TodoItemDto>(todoItem);

            return Ok(todoItemDto);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateTodoItemDto todoItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var tokenDto = _tokenService.EncodeToken(token);
            var todoItem = _mapper.Map<TodoItem>(todoItemDto);
            todoItem.UserId = tokenDto.UserId;

            await _todoItemRepository.CreateAsync(todoItem);

            return CreatedAtAction(nameof(GetById), new { id = todoItem.Id }, _mapper.Map<TodoItemDto>(todoItem));
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateTodoItemDto todoItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var tokenDto = _tokenService.EncodeToken(token);
            var isTodoItemMatchUser = await _todoItemRepository.IsTodoItemMatchUser(id, tokenDto.UserId);
            if (isTodoItemMatchUser == null)
            {
                return NotFound();
            }
            if (!(bool)isTodoItemMatchUser)
            {
                return BadRequest("User is not the owner");
            }

            var todoItem = await _todoItemRepository.UpdateAsync(id, todoItemDto);

            if (todoItem == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TodoItemDto>(todoItem));
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var tokenDto = _tokenService.EncodeToken(token);
            var isTodoItemMatchUser = await _todoItemRepository.IsTodoItemMatchUser(id, tokenDto.UserId);
            if (isTodoItemMatchUser == null)
            {
                return NotFound();
            }
            if (!(bool)isTodoItemMatchUser)
            {
                return BadRequest("User is not the owner");
            }


            var todoItem = await _todoItemRepository.DeleteAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}