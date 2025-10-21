using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stock_Social_Platform.Extensions;
using Stock_Social_Platform.Interfaces;
using Stock_Social_Platform.Models;

namespace Stock_Social_Platform.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController  : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepo;
        private readonly IPortfolioRepository _portfolioRepo;
        private readonly IFMPService _fmpService;

        public PortfolioController(IStockRepository stockRepo, UserManager<AppUser> userManager, IPortfolioRepository portfolioRepository, IFMPService fMPService)
        {
            _stockRepo = stockRepo;
            _userManager = userManager;
            _portfolioRepo = portfolioRepository;
            _fmpService = fMPService;
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(username);

            if (appUser == null) return NotFound("Cannot find user");

            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);

            return Ok(userPortfolio);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio([FromBody] string symbol)
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

            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);

            if (userPortfolio.Any(e => e.Symbol.Equals(symbol, StringComparison.CurrentCultureIgnoreCase)))
            {
                return BadRequest("Cannot add same stocks to the portfolio");
            }

            var portfolioModel = new Portfolio
            {
                AppUserId = appUser.Id,
                StockId = stock.Id
            };

            var createdPorfolio = await _portfolioRepo.Create(portfolioModel);

            return Created();

        }


        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string symbol)
        {
            var username = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(username);

            if (appUser == null) return NotFound("Cannot find user");

            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);

            var stock = userPortfolio.FirstOrDefault(s => s.Symbol.ToLower() == symbol.ToLower());

            if (stock == null) return NotFound("Cannot find stock from portfolio");

            bool deleteStatus = await _portfolioRepo.Delete(appUser, symbol);

            if (deleteStatus)
            {
                return NoContent();
            }
            else
            {
                return StatusCode(500);
            }

        }



        
    }
}