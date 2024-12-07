using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Okala.Application.Interfaces.IRepositories;
using Okala.Application.Services;
using Okala.Domain.DTOs;
using Okala.Domain.Request;
using Okala.Domain.Response;
using Okala.Domain.Settings;

namespace Okala.Test.Okala.Application
{
    public class CryptoServiceTest
    {
        [Fact]
        public async Task GetCryptoQuote_SuccessfulResult()
        {
            //Arrange
            var randomizer = new Bogus.Randomizer();
            var cryptoPriceRepository = new Mock<ICryptoPriceRepository>();
            cryptoPriceRepository.Setup(cpr => cpr.GetPriceAsync(It.IsAny<string>()))
                .ReturnsAsync(randomizer.Decimal(0, 1000));
            List<string> currencies =
            [
                "EUR",
                "BRL",
                "GBP",
                "AUD"
            ];
            var exchangeRateRepository = new Mock<IExchangeRateRepository>();
            exchangeRateRepository.Setup(er =>
                    er.ConvertFromUsdAsync(It.IsAny<decimal>()))
                .ReturnsAsync(currencies.Select(c => new KeyValuePair<string, decimal>(c, randomizer.Decimal(0, 1000)))
                    .ToDictionary);
            var symbol = "BTC";
            var currencySetting = new CurrencySetting { Currencies = currencies };
            var service = new CryptoService(cryptoPriceRepository.Object, exchangeRateRepository.Object,
                new OptionsWrapper<CurrencySetting>(currencySetting));

            // Act
            var result = await service.GetCryptoQuoteAsync(new CryptoQuoteRequest { CryptoSymbol = symbol });

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(DataResponse<CryptoQuoteResponse>));
            result.IsSuccess.Should().BeTrue();
            result.Data.Symbol.Should().Be(symbol);
            result.Data.ExchangeRates.Should().ContainKeys(currencies).And.ContainKey("USD");
        }
    }
}
