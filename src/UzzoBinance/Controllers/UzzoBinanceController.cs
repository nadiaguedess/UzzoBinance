using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using UzzoBinance.Models.DataBase;
using UzzoBinance.Models.UzzoBinance;
using UzzoBinance.Services;

namespace UzzoBinance.Controllers
{
    public class UzzoBinanceController : Controller
    {
        private readonly UzzoBinanceContext db_UzzoBinance;
        private IConfiguration _configuration;
        public UzzoBinanceController(IConfiguration Configuration, UzzoBinanceContext db_UzzoBinance)
        {
            this.db_UzzoBinance = db_UzzoBinance;
            _configuration = Configuration;
        }

        [HttpGet]
        public IActionResult UzzoBinance()
        {
            //Instanciando serviço que será utilizado
            UzzoPayService obj_ListSymbolPrice = new UzzoPayService(db_UzzoBinance);

            //Retornando para viewModel o resultado do select
            List<SymbolPrice> obj_ViewModel = obj_ListSymbolPrice.SelectLastPrices();

            //Retornando para view os últimos preços salvos
            return View(obj_ViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UzzoBinance(SearchPrice obj_SearchPrice)
        {
            BinanceService objBinanceService = new BinanceService(_configuration, db_UzzoBinance);
            await objBinanceService.SearchPrice();

            //Instanciando serviço que será utilizado
            UzzoPayService obj_ListSymbolPrice = new UzzoPayService(db_UzzoBinance);

            //Retornando para viewModel o resultado do select
            List<SymbolPrice> obj_ViewModel = obj_ListSymbolPrice.SelectLastPrices();

            //Retornando para view os últimos preços salvos
            return View(obj_ViewModel);
        }

        [HttpGet]
        public IActionResult UzzoBinancePartialView()
        {
            //Instanciando serviço que será utilizado
            UzzoPayService obj_ListSymbolPrice = new UzzoPayService(db_UzzoBinance);

            //Retornando para viewModel o resultado do select
            List<SymbolPrice> obj_ViewModel = obj_ListSymbolPrice.SelectLastPrices();

            //Retornando para view os últimos preços salvos
            return PartialView(obj_ViewModel);
        }

    }   

}
