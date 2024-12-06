using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Okala.Application.Interfaces.IRepositories;
using Okala.Domain.Exceptions;
using Okala.Domain.Redis;
using Okala.Domain.Response;
using Okala.Domain.Settings;

namespace Okala.Infrastructure.Repositories
{
    public class CoinMarketCapCryptoPriceRepository(
        IOptions<ApiKeySetting> apiKeySetting, 
        HttpClient httpClient,
        IRedisCacheService? redisCacheService = null) : ICryptoPriceRepository
    {
        private readonly string _baseUrl = "https://pro-api.coinmarketcap.com";
        public async Task<decimal> GetPriceAsync(string cryptoSymbol)
        {
            decimal? price = null;
            if (redisCacheService != null)
            {
                // Get cached value if exist
                price = await redisCacheService.GetCacheValueAsync<decimal?>(
                    $"CoinMarketCapCryptoPrice_{cryptoSymbol.ToUpper()}");
                if (price == -1)
                    throw new StatusCodeException([
                        new ErrorResponseDetail
                        {
                            ErrorId = null,
                            ErrorKey = nameof(cryptoSymbol),
                            ErrorMessage = "Crypto symbol is invalid.",
                            IsInternalError = false
                        }
                    ], HttpStatusCode.BadRequest);
            }

            if (price == null)
            {
                // Send request
                var request =
                    new HttpRequestMessage(HttpMethod.Get,
                        $"{_baseUrl}/v2/cryptocurrency/quotes/latest?symbol={cryptoSymbol}");
                request.Headers.Add("X-CMC_PRO_API_KEY", apiKeySetting.Value.CoinMarketCap);
                request.Headers.Add("Accept", "*/*");
                var response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                // Find and return price in USD
                var coins =
                    (JArray)((JObject)JsonConvert.DeserializeObject(content))["data"][cryptoSymbol.ToUpper()];
                if (coins.Count == 0)
                {
                    if (redisCacheService != null)
                        await redisCacheService.SetCacheValueAsync(
                            $"CoinMarketCapCryptoPrice_{cryptoSymbol.ToUpper()}", -1,
                            TimeSpan.FromDays(30));

                    throw new StatusCodeException([
                        new ErrorResponseDetail
                        {
                            ErrorId = null,
                            ErrorKey = nameof(cryptoSymbol),
                            ErrorMessage = "Crypto symbol is invalid.",
                            IsInternalError = false
                        }
                    ], HttpStatusCode.BadRequest);
                }

                price = (decimal)coins[0]["quote"]["USD"]["price"]!;

                if (redisCacheService != null)
                    await redisCacheService.SetCacheValueAsync($"CoinMarketCapCryptoPrice_{cryptoSymbol.ToUpper()}",
                        price, TimeSpan.FromMinutes(1));
            }

            return price.Value;
        }
    }
}
