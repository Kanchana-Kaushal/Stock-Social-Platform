using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
    }
}