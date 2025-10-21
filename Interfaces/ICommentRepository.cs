using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stock_Social_Platform.Dtos.Comment;
using Stock_Social_Platform.Helpers;
using Stock_Social_Platform.Models;

namespace Stock_Social_Platform.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllSync(QueryObjectComments query);
        Task<Comment?> GetByIdAsync(int id);
        Task<Comment> CreateAsync(Comment commentModel);
        Task<Comment?> UpdateAsync(UpdateCommentDto updateCommentDto, int id);
        Task<Comment?> DeleteAsync(int id);
    }
}