using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UzzoBinance.Models.UzzoBinance
{
    public class SearchPrice
    {
        public string symbol { get; set; } = "BTCUSDT";
        public double priceChange { get; set; }
        public double priceChangePercent { get; set; }
        public double weightedAvgPrice { get; set; }
        public double prevClosePrice { get; set; }
        public double lastPrice { get; set; }
        public double lastQty { get; set; }
        public double bidPrice { get; set; }
        public double bidQty { get; set; }
        public double askPrice { get; set; }
        public double askQty { get; set; }
        public double openPrice { get; set; }
        public double highPrice { get; set; }
        public double lowPrice { get; set; }
        public double volume { get; set; }
        public double quoteVolume { get; set; }
        public long openTime { get; set; }
        public long closeTime { get; set; }
        public int firstId { get; set; }
        public int lastId { get; set; }
        public int count { get; set; }
    }
}
