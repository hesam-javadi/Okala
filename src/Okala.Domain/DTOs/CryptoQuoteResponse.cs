using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okala.Domain.DTOs
{
    public class CryptoQuoteResponse
    {
        public string Symbol { get; set; } = null!;
        public Dictionary<string, decimal> ExchangeRates { get; set; } = null!;
    }
}
