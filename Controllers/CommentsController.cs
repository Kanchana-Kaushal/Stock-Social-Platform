using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stock_Social_Platform.Dtos.Comment;
using Stock_Social_Platform.Extensions;
using Stock_Social_Platform.Helpers;
using Stock_Social_Platform.Interfaces;
using Stock_Social_Platform.Mappers;
using Stock_Social_Platform.Models;

namespace Stock_Social_Platform.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFMPService _fmpService;

        public CommentsController(ICommentRepository commentRepo, IStockRepository stockRepo, UserManager<AppUser> userManager, IFMPService fMPService)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
            _userManager = userManager;
            _fmpService = fMPService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObjectComments queryObject)
        {
            var comments = await _commentRepo.GetAllSync(queryObject);
            var commentsDto = comments.Select(x => x.ToCommentDto());
            return Ok(commentsDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comment = await _commentRepo.GetByIdAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{symbol:alpha}")]
        [Authorize]
        public async Task<IActionResult> Create([FromRoute] string symbol, [FromBody] CreateCommentDto commentDto)
        {
            var username = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(username);

            if (appUser == null) return NotFound("Cannot find user");

            var stock = await _stockRepo.FindStockBySymbol(symbol);

            if(stock == null)
            {
                stock = await _fmpService.FindStockBySymbolAsync(symbol);
                if (stock == null)
                {
                    return BadRequest("This stock does not exists");
                }
                else
                {
                    await _stockRepo.CreateAsync(stock);
                }
            }

            var commentModel = commentDto.ToCommentFromCreateDto(stock.Id , appUser);
            var savedComment = await _commentRepo.CreateAsync(commentModel);


            return CreatedAtAction(nameof(GetById), new { id = savedComment.Id }, savedComment.ToCommentDto());

        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentDto commentDto)
        {
            var updatedComment = await _commentRepo.UpdateAsync(commentDto, id);

            if (updatedComment == null)
            {
                return NotFound("Comment does not exists");
            }

            return Ok(updatedComment.ToCommentDto());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var deletedComment =await _commentRepo.DeleteAsync(id);

            if (deletedComment == null)
            {
                return NotFound("Cannot find comment");
            }

            return NoContent();
        }
    }
} 