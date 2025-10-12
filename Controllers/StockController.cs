using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stock_Social_Platform.Data;
using Stock_Social_Platform.Dtos.Stock;
using Stock_Social_Platform.Interfaces;
using Stock_Social_Platform.Mappers;


namespace Stock_Social_Platform.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IStockRepository _stockRepo;
        public StockController(ApplicationDBContext context, IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stocks = await _stockRepo.GetStocksAsync();

            var stockDto = stocks.Select(s => s.ToStockDto());

            return Ok(stocks);
        }


        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById([FromRoute] int Id)
        {
            var stock = await _stockRepo.GetByIdAsync(Id);

            if (stock == null)
            {
                return NotFound("Cannot find the stock");
            }

            return Ok(stock.ToStockDto());
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            var stockModel = stockDto.ToStockFromCreateDTO();

            await _stockRepo.CreateAsync(stockModel);

            return CreatedAtAction(nameof(GetById), new { Id = stockModel.Id }, stockModel.ToStockDto());
        }


        [HttpPut("{Id}")]
        public async Task<IActionResult> Update([FromRoute] int Id, [FromBody] UpdateStockRequestDto updateDto)
        {
            var stockModel = await _stockRepo.UpdateAsync(Id, updateDto);

            if (stockModel == null) { return NotFound("Cannot find record"); }

            return Ok(stockModel.ToStockDto());
        }


        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] int Id)
        {
            var stockModel = await _stockRepo.DeleteSync(Id);

            if (stockModel == null){ return NotFound("Cannot find record"); }

            return NoContent();
            
        }  
    }
    
}