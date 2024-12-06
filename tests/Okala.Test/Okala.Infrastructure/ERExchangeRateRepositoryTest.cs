using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq.Protected;
using Moq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Okala.Domain.Settings;
using Okala.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Okala.Test.Okala.Infrastructure
{
    public class ERExchangeRateRepositoryTest
    {
        [Fact]
        public async Task GetPrice_SuccessfulResult()
        {
            // Arrange
            var randomizer = new Randomizer();
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            string[] responseString =
            [
                "{\"success\":true,\"timestamp\":1733479443,\"base\":\"EUR\",\"date\":\"2024-12-06\",\"rates\":{\"AED\":3.885081,\"AFN\":72.66167,\"ALL\":98.608506,\"AMD\":424.552301,\"ANG\":1.900513,\"AOA\":966.25391,\"ARS\":1070.700088,\"AUD\":1.64622,\"AWG\":1.90659,\"AZN\":1.800676,\"BAM\":1.957424,\"BBD\":2.129207,\"BDT\":126.014569,\"BGN\":1.955246,\"BHD\":0.398786,\"BIF\":3116.144752,\"BMD\":1.057748,\"BND\":1.4136,\"BOB\":7.287047,\"BRL\":6.392973,\"BSD\":1.054495,\"BTC\":1.0781547e-5,\"BTN\":89.296099,\"BWP\":14.367194,\"BYN\":3.451029,\"BYR\":20731.85566,\"BZD\":2.125604,\"CAD\":1.484867,\"CDF\":3035.736548,\"CHF\":0.929438,\"CLF\":0.037191,\"CLP\":1026.205949,\"CNY\":7.684115,\"CNH\":7.691148,\"COP\":4670.400516,\"CRC\":544.462099,\"CUC\":1.057748,\"CUP\":28.030315,\"CVE\":110.356576,\"CZK\":25.11855,\"DJF\":187.789382,\"DKK\":7.458148,\"DOP\":63.373787,\"DZD\":140.931736,\"EGP\":52.861893,\"ERN\":15.866216,\"ETB\":132.059766,\"EUR\":1,\"FJD\":2.409524,\"FKP\":0.834898,\"GBP\":0.829066,\"GEL\":2.972017,\"GGP\":0.834898,\"GHS\":15.707505,\"GIP\":0.834898,\"GMD\":75.633615,\"GNF\":9124.132152,\"GTQ\":8.13375,\"GYD\":220.623077,\"HKD\":8.228606,\"HNL\":26.70141,\"HRK\":7.545189,\"HTG\":138.157258,\"HUF\":413.330263,\"IDR\":16759.378256,\"ILS\":3.804634,\"IMP\":0.834898,\"INR\":89.578012,\"IQD\":1385.649536,\"IRR\":44531.179871,\"ISK\":145.493538,\"JEP\":0.834898,\"JMD\":165.438848,\"JOD\":0.750044,\"JPY\":159.244448,\"KES\":136.713974,\"KGS\":91.758113,\"KHR\":4263.781034,\"KMF\":492.910719,\"KPW\":951.972563,\"KRW\":1501.811119,\"KWD\":0.325151,\"KYD\":0.878746,\"KZT\":550.487213,\"LAK\":23164.675276,\"LBP\":94721.309879,\"LKR\":306.12982,\"LRD\":190.394226,\"LSL\":19.092306,\"LTL\":3.123253,\"LVL\":0.639821,\"LYD\":5.161506,\"MAD\":10.499736,\"MDL\":19.319214,\"MGA\":4960.836852,\"MKD\":61.543305,\"MMK\":3435.523392,\"MNT\":3594.226756,\"MOP\":8.453315,\"MRU\":40.591037,\"MUR\":49.164248,\"MVR\":16.299948,\"MWK\":1840.480916,\"MXN\":21.402626,\"MYR\":4.674717,\"MZN\":67.600489,\"NAD\":19.092008,\"NGN\":1699.163819,\"NIO\":38.845773,\"NOK\":11.689133,\"NPR\":142.874911,\"NZD\":1.807574,\"OMR\":0.407232,\"PAB\":1.054575,\"PEN\":3.942271,\"PGK\":4.212477,\"PHP\":61.231895,\"PKR\":293.868737,\"PLN\":4.267191,\"PYG\":8249.00114,\"QAR\":3.850995,\"RON\":4.977231,\"RSD\":116.94486,\"RUB\":105.617589,\"RWF\":1467.096112,\"SAR\":3.973636,\"SBD\":8.867686,\"SCR\":14.427425,\"SDG\":636.246715,\"SEK\":11.486437,\"SGD\":1.417483,\"SHP\":0.834898,\"SLE\":24.106748,\"SLL\":22180.446196,\"SOS\":602.705834,\"SRD\":37.334222,\"STD\":21893.242587,\"SVC\":9.226831,\"SYP\":2657.622774,\"SZL\":19.092443,\"THB\":36.053337,\"TJS\":11.520451,\"TMT\":3.702117,\"TND\":3.33767,\"TOP\":2.477352,\"TRY\":36.805922,\"TTD\":7.144063,\"TWD\":34.255154,\"TZS\":2750.144169,\"UAH\":43.844812,\"UGX\":3869.210739,\"USD\":1.057748,\"UYU\":43.725871,\"UZS\":13576.192555,\"VES\":51.053434,\"VND\":26856.215061,\"VUV\":125.577937,\"WST\":2.952799,\"XAF\":656.514201,\"XAG\":0.033802,\"XAU\":0.000401,\"XCD\":2.858616,\"XDR\":0.802081,\"XOF\":658.448007,\"XPF\":119.331742,\"YER\":264.807181,\"ZAR\":19.071774,\"ZMK\":9520.998442,\"ZMW\":28.551232,\"ZWL\":340.59434}}"
            ];
            var currencySetting = new CurrencySetting
            {
                Currencies = [
                    "EUR",
                    "BRL",
                    "GBP",
                    "AUD"
                ]
            };
            var expectedResult = currencySetting.Currencies.Select(c =>
                    new KeyValuePair<string, decimal>(c,
                        (decimal)((JObject)JsonConvert.DeserializeObject(responseString[0]))["rates"][c]))
                .ToDictionary();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseString[0])
                })
                .Verifiable();
            var httpClient = new HttpClient(handlerMock.Object);
            var apiKeySetting = new ApiKeySetting()
            {
                ExchangeRates = randomizer.String2(36, 36)
            };
            var erExchangeRateRepository =
                new ERExchangeRateRepository(new OptionsWrapper<ApiKeySetting>(apiKeySetting),
                    new OptionsWrapper<CurrencySetting>(currencySetting), httpClient);

            // Assert
            var result = await erExchangeRateRepository.ConvertFromUsdAsync(1);

            // Arrange
            result.Should().Equal(expectedResult);
        }

        [Fact]
        public async Task GetPrice_UnsuccessfulResult()
        {
            // Arrange
            var randomizer = new Randomizer();
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            string[] responseString =
            [
                "{\"error\":{\"code\":\"invalid_access_key\",\"message\":\"You have not supplied a valid API Access Key. [Technical Support: support@apilayer.com]\"}}"
            ];
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    Content = new StringContent(responseString[0])
                })
                .Verifiable();
            var httpClient = new HttpClient(handlerMock.Object);
            var apiKeySetting = new ApiKeySetting()
            {
                ExchangeRates = randomizer.String2(36, 36)
            };
            var currencySetting = new CurrencySetting
            {
                Currencies = [
                    "EUR",
                    "BRL",
                    "GBP",
                    "AUD"
                ]
            };
            var erExchangeRateRepository =
                new ERExchangeRateRepository(new OptionsWrapper<ApiKeySetting>(apiKeySetting),
                    new OptionsWrapper<CurrencySetting>(currencySetting), httpClient);

            // Assert
            Action action = () => _ = erExchangeRateRepository.ConvertFromUsdAsync(1).Result;

            // Arrange
            action.Should().Throw<HttpRequestException>()
                .Where(e => e.StatusCode == HttpStatusCode.Unauthorized);
        }
    }
}
