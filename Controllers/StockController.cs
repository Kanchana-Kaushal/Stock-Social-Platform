using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stock_Social_Platform.Data;
using Stock_Social_Platform.Dtos.Stock;
using Stock_Social_Platform.Mappers;

namespace Stock_Social_Platform.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController: ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public StockController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var stocks = _context.Stock.ToList().Select(s => s.ToStockDto());
            return Ok(stocks);
        }


        [HttpGet("{Id}")]
        public IActionResult GetById([FromRoute] int Id)
        {
            var stock = _context.Stock.Find(Id);

            if (stock == null)
            {
                return NotFound("Cannot find the stock");
            }

            return Ok(stock.ToStockDto());
        }


        [HttpPost]
        public IActionResult Create([FromBody] CreateStockRequestDto stockDto)
        {
            var stockModel = stockDto.ToStockFromCreateDTO();
            _context.Stock.Add(stockModel);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { Id = stockModel.Id }, stockModel.ToStockDto());

        }

        [HttpPut("{Id}")]
        public IActionResult Update([FromRoute] int Id, [FromBody] UpdateStockRequestDto updateDto)
        {
            var stockModel = _context.Stock.FirstOrDefault(x => x.Id == Id);

            if (stockModel == null)
            {
                return NotFound("Cannot find record");
            }

            stockModel.Symbol = updateDto.Symbol;
            stockModel.CompanyName = updateDto.CompanyName;
            stockModel.Purchase = updateDto.Purchase;
            stockModel.LastDiv = updateDto.LastDiv;
            stockModel.Industry = updateDto.Industry;
            stockModel.MarketCap = updateDto.MarketCap;

            _context.SaveChanges();

            return Ok(stockModel.ToStockDto());
        }


        [HttpDelete("{Id}")]
        public IActionResult Delete([FromRoute] int Id)
        {
            var stockModel = _context.Stock.FirstOrDefault(x => x.Id == Id);

            if (stockModel == null)
            {
                return NotFound("Cannot find record");
            }

            _context.Stock.Remove(stockModel);
            _context.SaveChanges();

            return NoContent();
        }
    }
}