using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stock_Social_Platform.Dtos.Comment;
using Stock_Social_Platform.Dtos.Stock.Comment;
using Stock_Social_Platform.Models;

namespace Stock_Social_Platform.Mappers
{
    public static class CommentMapper
    {
        public static CommentDto ToCommentDto(this Comment commentModel)
        {
            return new CommentDto
            {
                Id = commentModel.Id,
                Title = commentModel.Title,
                StockId = commentModel.StockId,
                Content = commentModel.Content,
                CreatedOn = commentModel.CreatedOn,
                CreatedBy = commentModel.AppUser.UserName,
            };
        }

        public static Comment ToCommentFromCreateDto(this CreateCommentDto commentDto, int stockId, AppUser appUser)
        {
            return new Comment
            {
                StockId = stockId,
                Title = commentDto.Title,
                Content = commentDto.Content,
                AppUserId = appUser.Id
            };
        }

    }
}
