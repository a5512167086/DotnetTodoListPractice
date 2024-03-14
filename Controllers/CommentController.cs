using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using PracticeProject.Dtos.Comment;
using PracticeProject.Helpers;
using PracticeProject.Interfaces;
using PracticeProject.Models;
using PracticeProject.Service;

namespace PracticeProject.Controllers
{
    [ApiController]
    [Route("api/comment")]
    [AllowAnonymous]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ITodoItemRepository _todoItemRepository;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public CommentController(ICommentRepository commentRepository, ITodoItemRepository todoItemRepository, ITokenService tokenService, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _todoItemRepository = todoItemRepository;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery]CommentQueryObject query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comments = await _commentRepository.GetAllAsync(query);
            var commentDtos = _mapper.Map<List<CommentDto>>(comments);

            return Ok(commentDtos);
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comment = await _commentRepository.GetByIdAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            var commentDto = _mapper.Map<CommentDto>(comment);

            return Ok(commentDto);
        }


        [HttpPost("{todoItemId:int}")]
        [Authorize]
        public async Task<ActionResult> Create([FromRoute] int todoItemId, [FromBody] CreateCommentDto commentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isTodoItemExists = await _todoItemRepository.IsTodoItemExists(todoItemId);
            if (!isTodoItemExists)
            {
                return BadRequest("TodoItem does not exist");
            }

            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var tokenDto = _tokenService.EncodeToken(token);
            var comment = _mapper.Map<Comment>(commentDto);
            comment.TodoItemId = todoItemId;
            comment.UserId = tokenDto.UserId;
            await _commentRepository.CreateAsync(comment);

            return CreatedAtAction(nameof(GetById), new { id = comment.Id }, _mapper.Map<CommentDto>(comment));
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentDto commentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var tokenDto = _tokenService.EncodeToken(token);
            var isCommentMatchUser = await _commentRepository.IsCommentMatchUser(id, tokenDto.UserId);
            if (isCommentMatchUser == null)
            {
                return NotFound();
            }
            if (!(bool)isCommentMatchUser)
            {
                return BadRequest("User is not the owner");
            }

            var comment = await _commentRepository.UpdateAsync(id, commentDto);

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CommentDto>(comment));
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var tokenDto = _tokenService.EncodeToken(token);
            var isCommentMatchUser = await _commentRepository.IsCommentMatchUser(id, tokenDto.UserId);
            if (isCommentMatchUser == null)
            {
                return NotFound();
            }
            if (!(bool)isCommentMatchUser)
            {
                return BadRequest("User is not the owner");
            }

            var comment = await _commentRepository.DeleteAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}