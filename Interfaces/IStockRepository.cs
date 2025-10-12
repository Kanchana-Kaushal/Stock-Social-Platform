using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stock_Social_Platform.Dtos.Stock;
using Stock_Social_Platform.Models;

namespace Stock_Social_Platform.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetStocksAsync();
        Task<Stock?> GetByIdAsync(int id);
        Task<Stock> CreateAsync(Stock stockModel);
        Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto updatekDto);
        Task<Stock?> DeleteSync(int id);

    }
}