using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stock_Social_Platform.Models;

namespace Stock_Social_Platform.Interfaces
{
    public interface IPortfolioRepository
    {
        Task<List<Stock>> GetUserPortfolio(AppUser user);
        Task<Portfolio> Create(Portfolio portfolioModel);
        Task<bool> Delete(AppUser appUser, string symbol);
    }
}