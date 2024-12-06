using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okala.Application.Interfaces.IRepositories
{
    public interface ICryptoPriceRepository
    {
        Task<decimal> GetPriceAsync(string cryptoSymbol);
    }
}
