using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Okala.Application.Interfaces.IRepositories;
using Okala.Application.Interfaces.IServices;
using Okala.Domain.DTOs;
using Okala.Domain.Request;
using Okala.Domain.Response;
using Okala.Domain.Settings;

namespace Okala.Application.Services
{
    public class CryptoService(
        ICryptoPriceRepository cryptoPriceRepository,
        IExchangeRateRepository exchangeRateRepository,
        IOptions<CurrencySetting> currencySetting) : ICryptoService
    {
        public async Task<DataResponse<CryptoQuoteResponse>> GetCryptoQuoteAsync(CryptoQuoteRequest request)
        {
            // Get crypto price in USD
            var cryptoPriceInUsd = await cryptoPriceRepository.GetPriceAsync(request.CryptoSymbol);

            var ret = new CryptoQuoteResponse()
            {
                Symbol = request.CryptoSymbol,
                ExchangeRates = new Dictionary<string, decimal> { { "USD", cryptoPriceInUsd } }
            };

            // Get Price in other currencies
            var convertedPrices =
                await exchangeRateRepository.ConvertFromUsdAsync(cryptoPriceInUsd);
            foreach (var convertedPrice in convertedPrices)
            {
                ret.ExchangeRates.Add(convertedPrice.Key, convertedPrice.Value);
            }

            return new DataResponse<CryptoQuoteResponse>(ret);
        }
    }
}
