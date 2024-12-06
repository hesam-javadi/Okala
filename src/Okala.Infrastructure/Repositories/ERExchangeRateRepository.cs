using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Okala.Application.Interfaces.IRepositories;
using Okala.Domain.Exceptions;
using Okala.Domain.Redis;
using Okala.Domain.Response;
using Okala.Domain.Settings;

namespace Okala.Infrastructure.Repositories
{
    public class ERExchangeRateRepository(
        IOptions<ApiKeySetting> apiKeySetting,
        IOptions<CurrencySetting> currencySetting,
        HttpClient httpClient,
        IRedisCacheService? redisCacheService = null) : IExchangeRateRepository
    {
        private readonly string _baseUrl = "https://api.exchangeratesapi.io";
        public async Task<Dictionary<string, decimal>> ConvertFromUsdAsync(decimal value)
        {
            Dictionary<string, decimal>? ret = null;
            if (redisCacheService != null)
            {
                // Get cached value if exist
                ret = await redisCacheService.GetCacheValueAsync<Dictionary<string, decimal>?>(
                    "ERExchangeRate");
            }

            if (ret == null)
            {
                // Send request
                var request =
                    new HttpRequestMessage(HttpMethod.Get,
                        $"{_baseUrl}/v1/latest?access_key={apiKeySetting.Value.ExchangeRates}");
                var response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                // Find and return price in USD
                var rates = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<JObject>(content)["rates"]);
                ret = JsonConvert.DeserializeObject<Dictionary<string, decimal>>(rates);

                if (redisCacheService != null)
                    await redisCacheService.SetCacheValueAsync("ERExchangeRate",
                        ret, TimeSpan.FromHours(1));
            }

            return ret.Where(r => currencySetting.Value.Currencies.Contains(r.Key))
                .Select(r => new KeyValuePair<string, decimal>(r.Key, r.Value * value)).ToDictionary();
        }
    }
}
