using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UzzoBinance.Models.UzzoBinance
{
    public class NewOrder
    {
        public string symbol { get; set; } = "BTCUSDT";
        public string side { get; set; } = "SELL";
        public string type { get; set; } = "LIMIT";
        public string timeInForce { get; set; } = "GTC";
        public double quantity { get; set; } = 0.01;
        public double price { get; set; } = 9000;
        public string newClientOrderId { get; set; }
        public long timestamp { get; set; }
        public string signature { get; set; }
        public int recvWindow { get; set; } = 60000;

    }
}
