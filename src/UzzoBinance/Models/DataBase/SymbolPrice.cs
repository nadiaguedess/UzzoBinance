using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UzzoBinance.Models.DataBase
{
    public class SymbolPrice
    {
        [Key]
        public DateTime dateAndTimePrice { get; set; }
        public string symbol { get; set; }
        public decimal bidPrice { get; set; }
        public decimal askPrice { get; set; }
        
    }
}
