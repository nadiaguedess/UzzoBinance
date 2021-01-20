using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using UzzoBinance.Models.DataBase;

namespace UzzoBinance.Services
{
    public class UzzoPayService
    {
        private readonly UzzoBinanceContext db_UzzoBinance;
        public UzzoPayService(UzzoBinanceContext db_UzzoBinance)
        {
            this.db_UzzoBinance = db_UzzoBinance;
        }

        //Metodo que retorna os ultimos  25 preços obtidos
        public List<SymbolPrice> SelectLastPrices()
        {
            //Instancia ViewModel 
            List<SymbolPrice> listSymbolPrice = new List<SymbolPrice>();

            //selecionar os últimos 25 preços cadastrados
            var listSymbolSelect = db_UzzoBinance.SymbolPrice 
                                                    .Select(SP => SP)                                       
                                                    .OrderByDescending(SP => SP.dateAndTimePrice)                                                    
                                                    .Take(25)
                                                    .ToList();

            foreach (var item in listSymbolSelect)
            {
                SymbolPrice columnSymbolPrice = new SymbolPrice();

                columnSymbolPrice.symbol = item.symbol;
                columnSymbolPrice.bidPrice = item.bidPrice;
                columnSymbolPrice.askPrice = item.askPrice;
                columnSymbolPrice.dateAndTimePrice = item.dateAndTimePrice;

                listSymbolPrice.Add(columnSymbolPrice);
            }


            //Retorna o resultado da seleção
            return listSymbolPrice;
        }


    }
}
