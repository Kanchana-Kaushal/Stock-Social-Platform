using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Stock_Social_Platform.Data;
using Stock_Social_Platform.Interfaces;
using Stock_Social_Platform.Models;

namespace Stock_Social_Platform.Repository
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDBContext _context;
        public PortfolioRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Portfolio> Create(Portfolio portfolioModel)
        {
            _context.Portfolio.Add(portfolioModel);
            await _context.SaveChangesAsync();
            return portfolioModel;
        }

        public async Task<bool> Delete(AppUser appUser, string symbol)
        {
            var stock = await _context.Portfolio.FirstOrDefaultAsync
                (p => p.AppUserId == appUser.Id && p.Stock.Symbol.ToLower() == symbol.ToLower());

            if (stock == null) return false;

            _context.Portfolio.Remove(stock);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Stock>> GetUserPortfolio(AppUser user)
        {
            return await _context.Portfolio.Where(u => u.AppUserId == user.Id)
            .Select(stock => new Stock
            {
                Id = stock.StockId,
                Symbol = stock.Stock.Symbol,
                CompanyName = stock.Stock.CompanyName,
                Purchase = stock.Stock.Purchase,
                LastDiv = stock.Stock.LastDiv,
                Industry = stock.Stock.Industry,
                MarketCap = stock.Stock.MarketCap
                
            }).ToListAsync();
        }
    }
}