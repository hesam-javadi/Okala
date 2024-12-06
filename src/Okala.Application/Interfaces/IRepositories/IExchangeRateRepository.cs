using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okala.Application.Interfaces.IRepositories
{
    public interface IExchangeRateRepository
    {
        Task<Dictionary<string, decimal>> ConvertFromUsdAsync(decimal value);
    }
}
