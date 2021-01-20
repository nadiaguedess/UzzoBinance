using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UzzoBinance.Models.DataBase
{
    public class UzzoBinanceContext : DbContext
    {
        public UzzoBinanceContext(DbContextOptions<UzzoBinanceContext> options) : base(options)
        {

        }

        public DbSet<SymbolPrice> SymbolPrice { get; set; }

        
    }
}
