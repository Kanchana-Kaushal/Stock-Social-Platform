using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Stock_Social_Platform.Data;
using Stock_Social_Platform.Dtos.Comment;
using Stock_Social_Platform.Helpers;
using Stock_Social_Platform.Interfaces;
using Stock_Social_Platform.Mappers;
using Stock_Social_Platform.Models;

namespace Stock_Social_Platform.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private ApplicationDBContext _context;

        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Comment> CreateAsync(Comment commentModel)
        {
            await _context.Comment.AddAsync(commentModel);
            await _context.SaveChangesAsync();

            return commentModel;
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var commentModel =await _context.Comment.FindAsync(id);

            if (commentModel == null) return null;

            _context.Comment.Remove(commentModel);
            await _context.SaveChangesAsync();

            return commentModel;
        }

        public async Task<List<Comment>> GetAllSync(QueryObjectComments query)
        {
            var comments = _context.Comment.Include(c => c.AppUser).Include(c => c.Stock).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                comments = comments.Where(c => c.Stock != null && c.Stock.Symbol.ToLower().Contains(query.Symbol.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy) && query.SortBy == "Date")
            {
                comments = query.IsDecending ? comments.OrderByDescending(c => c.CreatedOn) : comments.OrderBy(c => c.CreatedOn);
            }


            int skip = (query.PageNumber - 1) * query.PageSize;

            return await comments.Skip(skip).Take(query.PageSize).ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comment.Include(c => c.AppUser).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Comment?> UpdateAsync(UpdateCommentDto updateCommentDto, int id)
        {
            var commentExists = await _context.Comment.FindAsync(id);

            if (commentExists == null) return null;

            commentExists.Content = updateCommentDto.Content;
            commentExists.Title = updateCommentDto.Title;

            await _context.SaveChangesAsync();

            return commentExists;
        }
    }
}