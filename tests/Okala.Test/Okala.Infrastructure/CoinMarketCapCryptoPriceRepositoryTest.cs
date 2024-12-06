using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Castle.Components.DictionaryAdapter.Xml;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Okala.Domain.Settings;
using Okala.Infrastructure.Repositories;

namespace Okala.Test.Okala.Infrastructure
{
    public class CoinMarketCapCryptoPriceRepositoryTest
    {
        [Fact]
        public async Task GetPrice_SuccessfulResult()
        {
            // Arrange
            var randomizer = new Randomizer();
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            string[] responseString =
            [
                "{\"status\":{\"timestamp\":\"2024-12-06T07:16:58.766Z\",\"error_code\":0,\"error_message\":null,\"elapsed\":25,\"credit_count\":1,\"notice\":null},\"data\":{\"BTC\":[{\"id\":1,\"name\":\"Bitcoin\",\"symbol\":\"BTC\",\"slug\":\"bitcoin\",\"num_market_pairs\":11823,\"date_added\":\"2010-07-13T00:00:00.000Z\",\"tags\":[{\"slug\":\"mineable\",\"name\":\"Mineable\",\"category\":\"OTHERS\"},{\"slug\":\"pow\",\"name\":\"PoW\",\"category\":\"ALGORITHM\"},{\"slug\":\"sha-256\",\"name\":\"SHA-256\",\"category\":\"ALGORITHM\"},{\"slug\":\"store-of-value\",\"name\":\"Store Of Value\",\"category\":\"CATEGORY\"},{\"slug\":\"state-channel\",\"name\":\"State Channel\",\"category\":\"CATEGORY\"},{\"slug\":\"coinbase-ventures-portfolio\",\"name\":\"Coinbase Ventures Portfolio\",\"category\":\"CATEGORY\"},{\"slug\":\"three-arrows-capital-portfolio\",\"name\":\"Three Arrows Capital Portfolio\",\"category\":\"CATEGORY\"},{\"slug\":\"polychain-capital-portfolio\",\"name\":\"Polychain Capital Portfolio\",\"category\":\"CATEGORY\"},{\"slug\":\"binance-labs-portfolio\",\"name\":\"Binance Labs Portfolio\",\"category\":\"CATEGORY\"},{\"slug\":\"blockchain-capital-portfolio\",\"name\":\"Blockchain Capital Portfolio\",\"category\":\"CATEGORY\"},{\"slug\":\"boostvc-portfolio\",\"name\":\"BoostVC Portfolio\",\"category\":\"CATEGORY\"},{\"slug\":\"cms-holdings-portfolio\",\"name\":\"CMS Holdings Portfolio\",\"category\":\"CATEGORY\"},{\"slug\":\"dcg-portfolio\",\"name\":\"DCG Portfolio\",\"category\":\"CATEGORY\"},{\"slug\":\"dragonfly-capital-portfolio\",\"name\":\"DragonFly Capital Portfolio\",\"category\":\"CATEGORY\"},{\"slug\":\"electric-capital-portfolio\",\"name\":\"Electric Capital Portfolio\",\"category\":\"CATEGORY\"},{\"slug\":\"fabric-ventures-portfolio\",\"name\":\"Fabric Ventures Portfolio\",\"category\":\"CATEGORY\"},{\"slug\":\"framework-ventures-portfolio\",\"name\":\"Framework Ventures Portfolio\",\"category\":\"CATEGORY\"},{\"slug\":\"galaxy-digital-portfolio\",\"name\":\"Galaxy Digital Portfolio\",\"category\":\"CATEGORY\"},{\"slug\":\"huobi-capital-portfolio\",\"name\":\"Huobi Capital Portfolio\",\"category\":\"CATEGORY\"},{\"slug\":\"alameda-research-portfolio\",\"name\":\"Alameda Research Portfolio\",\"category\":\"CATEGORY\"},{\"slug\":\"a16z-portfolio\",\"name\":\"a16z Portfolio\",\"category\":\"CATEGORY\"},{\"slug\":\"1confirmation-portfolio\",\"name\":\"1Confirmation Portfolio\",\"category\":\"CATEGORY\"},{\"slug\":\"winklevoss-capital-portfolio\",\"name\":\"Winklevoss Capital Portfolio\",\"category\":\"CATEGORY\"},{\"slug\":\"usv-portfolio\",\"name\":\"USV Portfolio\",\"category\":\"CATEGORY\"},{\"slug\":\"placeholder-ventures-portfolio\",\"name\":\"Placeholder Ventures Portfolio\",\"category\":\"CATEGORY\"},{\"slug\":\"pantera-capital-portfolio\",\"name\":\"Pantera Capital Portfolio\",\"category\":\"CATEGORY\"},{\"slug\":\"multicoin-capital-portfolio\",\"name\":\"Multicoin Capital Portfolio\",\"category\":\"CATEGORY\"},{\"slug\":\"paradigm-portfolio\",\"name\":\"Paradigm Portfolio\",\"category\":\"CATEGORY\"},{\"slug\":\"bitcoin-ecosystem\",\"name\":\"Bitcoin Ecosystem\",\"category\":\"PLATFORM\"},{\"slug\":\"ftx-bankruptcy-estate\",\"name\":\"FTX Bankruptcy Estate \",\"category\":\"CATEGORY\"},{\"slug\":\"2017-2018-alt-season\",\"name\":\"2017/18 Alt season\",\"category\":\"CATEGORY\"}],\"max_supply\":21000000,\"circulating_supply\":19790568,\"total_supply\":19790568,\"is_active\":1,\"infinite_supply\":false,\"platform\":null,\"cmc_rank\":1,\"is_fiat\":0,\"self_reported_circulating_supply\":null,\"self_reported_market_cap\":null,\"tvl_ratio\":null,\"last_updated\":\"2024-12-06T07:15:00.000Z\",\"quote\":{\"USD\":{\"price\":97864.8913492631,\"volume_24h\":124362247862.87567,\"volume_change_24h\":5.3965,\"percent_change_1h\":-0.1783272,\"percent_change_24h\":-3.70574513,\"percent_change_7d\":1.65047295,\"percent_change_30d\":31.30084875,\"percent_change_60d\":53.87258922,\"percent_change_90d\":80.17321428,\"market_cap\":1936801787060.2031,\"market_cap_dominance\":53.8655,\"fully_diluted_market_cap\":2055162718334.53,\"tvl\":null,\"last_updated\":\"2024-12-06T07:15:00.000Z\"}}},{\"id\":34316,\"name\":\"HarryPotterTrumpSonic100Inu\",\"symbol\":\"BTC\",\"slug\":\"harrypottertrumpsonic100inu\",\"num_market_pairs\":1,\"date_added\":\"2024-11-29T10:04:10.000Z\",\"tags\":[{\"slug\":\"memes\",\"name\":\"Memes\",\"category\":\"INDUSTRY\"}],\"max_supply\":1000420069,\"circulating_supply\":0,\"total_supply\":1000420069,\"platform\":{\"id\":1027,\"name\":\"Ethereum\",\"symbol\":\"ETH\",\"slug\":\"ethereum\",\"token_address\":\"0x7099aB9E42Fa7327a6b15E0a0c120c3e50d11BeC\"},\"is_active\":1,\"infinite_supply\":false,\"cmc_rank\":3772,\"is_fiat\":0,\"self_reported_circulating_supply\":1000420069,\"self_reported_market_cap\":1296262.8846426732,\"tvl_ratio\":null,\"last_updated\":\"2024-12-06T07:16:00.000Z\",\"quote\":{\"USD\":{\"price\":0.00129571859342885,\"volume_24h\":212522.90318737,\"volume_change_24h\":177.2069,\"percent_change_1h\":15.24370022,\"percent_change_24h\":79.94256809,\"percent_change_7d\":13.4753843,\"percent_change_30d\":13.4753843,\"percent_change_60d\":13.4753843,\"percent_change_90d\":13.4753843,\"market_cap\":0,\"market_cap_dominance\":0,\"fully_diluted_market_cap\":1296262.88,\"tvl\":null,\"last_updated\":\"2024-12-06T07:16:00.000Z\"}}},{\"id\":30938,\"name\":\"Satoshi Pumpomoto\",\"symbol\":\"BTC\",\"slug\":\"satoshi-pumpomoto\",\"num_market_pairs\":2,\"date_added\":\"2024-04-26T06:54:08.000Z\",\"tags\":[],\"max_supply\":null,\"circulating_supply\":0,\"total_supply\":21000000,\"platform\":{\"id\":5426,\"name\":\"Solana\",\"symbol\":\"SOL\",\"slug\":\"solana\",\"token_address\":\"6AGNtEgBE2jph1bWFdyaqsXJ762emaP9RE17kKxEsfiV\"},\"is_active\":1,\"infinite_supply\":false,\"cmc_rank\":6685,\"is_fiat\":0,\"self_reported_circulating_supply\":21000000,\"self_reported_market_cap\":6093.926040471958,\"tvl_ratio\":null,\"last_updated\":\"2024-12-06T07:16:00.000Z\",\"quote\":{\"USD\":{\"price\":0.00029018695430818846,\"volume_24h\":1005.44923573,\"volume_change_24h\":1180.2361,\"percent_change_1h\":0,\"percent_change_24h\":4.15524948,\"percent_change_7d\":-10.68615866,\"percent_change_30d\":6.98849051,\"percent_change_60d\":44.72403325,\"percent_change_90d\":38.60025595,\"market_cap\":0,\"market_cap_dominance\":0,\"fully_diluted_market_cap\":6093.93,\"tvl\":null,\"last_updated\":\"2024-12-06T07:16:00.000Z\"}}},{\"id\":31652,\"name\":\"batcat\",\"symbol\":\"BTC\",\"slug\":\"batcat\",\"num_market_pairs\":4,\"date_added\":\"2024-06-06T09:25:57.000Z\",\"tags\":[{\"slug\":\"memes\",\"name\":\"Memes\",\"category\":\"INDUSTRY\"},{\"slug\":\"solana-ecosystem\",\"name\":\"Solana Ecosystem\",\"category\":\"PLATFORM\"}],\"max_supply\":null,\"circulating_supply\":0,\"total_supply\":1000000000,\"platform\":{\"id\":5426,\"name\":\"Solana\",\"symbol\":\"SOL\",\"slug\":\"solana\",\"token_address\":\"EtBc6gkCvsB9c6f5wSbwG8wPjRqXMB5euptK6bqG1R4X\"},\"is_active\":1,\"infinite_supply\":false,\"cmc_rank\":6951,\"is_fiat\":0,\"self_reported_circulating_supply\":999939377,\"self_reported_market_cap\":101216.20557221514,\"tvl_ratio\":null,\"last_updated\":\"2024-12-06T07:16:00.000Z\",\"quote\":{\"USD\":{\"price\":0.00010122234197425264,\"volume_24h\":624.24532928,\"volume_change_24h\":-66.4513,\"percent_change_1h\":0,\"percent_change_24h\":0.61147818,\"percent_change_7d\":-1.9975334,\"percent_change_30d\":-25.06225932,\"percent_change_60d\":19.18955968,\"percent_change_90d\":86.77092591,\"market_cap\":0,\"market_cap_dominance\":0,\"fully_diluted_market_cap\":101222.34,\"tvl\":null,\"last_updated\":\"2024-12-06T07:16:00.000Z\"}}},{\"id\":32295,\"name\":\"Bullish Trump Coin\",\"symbol\":\"BTC\",\"slug\":\"bullish-trump-coin\",\"num_market_pairs\":1,\"date_added\":\"2024-07-19T09:39:13.000Z\",\"tags\":[{\"slug\":\"memes\",\"name\":\"Memes\",\"category\":\"INDUSTRY\"},{\"slug\":\"political-memes\",\"name\":\"Political Memes\",\"category\":\"CATEGORY\"}],\"max_supply\":420690000000,\"circulating_supply\":0,\"total_supply\":420690000000,\"platform\":{\"id\":1027,\"name\":\"Ethereum\",\"symbol\":\"ETH\",\"slug\":\"ethereum\",\"token_address\":\"0x43FD9De06bb69aD771556E171f960A91c42D2955\"},\"is_active\":1,\"infinite_supply\":false,\"cmc_rank\":7742,\"is_fiat\":0,\"self_reported_circulating_supply\":420690000000,\"self_reported_market_cap\":50354.497397409395,\"tvl_ratio\":null,\"last_updated\":\"2024-12-06T07:16:00.000Z\",\"quote\":{\"USD\":{\"price\":1.196950186536628e-7,\"volume_24h\":150.8185643,\"volume_change_24h\":0,\"percent_change_1h\":0,\"percent_change_24h\":3.44867852,\"percent_change_7d\":9.0413098,\"percent_change_30d\":-47.3007896,\"percent_change_60d\":16.31022974,\"percent_change_90d\":3.16648232,\"market_cap\":0,\"market_cap_dominance\":0,\"fully_diluted_market_cap\":50354.5,\"tvl\":null,\"last_updated\":\"2024-12-06T07:16:00.000Z\"}}},{\"id\":31469,\"name\":\"Boost Trump Campaign\",\"symbol\":\"BTC\",\"slug\":\"boost-trump-campaign\",\"num_market_pairs\":3,\"date_added\":\"2024-05-27T11:52:59.000Z\",\"tags\":[],\"max_supply\":420690000000,\"circulating_supply\":0,\"total_supply\":420690000000,\"platform\":{\"id\":1027,\"name\":\"Ethereum\",\"symbol\":\"ETH\",\"slug\":\"ethereum\",\"token_address\":\"0x300e0d87f8c95d90cfe4b809baa3a6c90e83b850\"},\"is_active\":1,\"infinite_supply\":false,\"cmc_rank\":9966,\"is_fiat\":0,\"self_reported_circulating_supply\":420690000000,\"self_reported_market_cap\":45776.37453117625,\"tvl_ratio\":null,\"last_updated\":\"2024-12-06T07:16:00.000Z\",\"quote\":{\"USD\":{\"price\":1.0881260436705472e-7,\"volume_24h\":0,\"volume_change_24h\":0,\"percent_change_1h\":0,\"percent_change_24h\":0,\"percent_change_7d\":-9.63658634,\"percent_change_30d\":-33.7208954,\"percent_change_60d\":-20.52983414,\"percent_change_90d\":-2.00657108,\"market_cap\":0,\"market_cap_dominance\":0,\"fully_diluted_market_cap\":45776.37,\"tvl\":null,\"last_updated\":\"2024-12-06T07:16:00.000Z\"}}}]}}"
            ];
            var expectedPrice =
                (decimal)((JObject)JsonConvert.DeserializeObject(responseString[0]))["data"]["BTC"][0]
                ["quote"]["USD"]["price"];
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
                CoinMarketCap = randomizer.String2(36, 36)
            };
            var coinMarketCapPriceRepository =
                new CoinMarketCapCryptoPriceRepository(new OptionsWrapper<ApiKeySetting>(apiKeySetting), httpClient);

            // Assert
            var result = await coinMarketCapPriceRepository.GetPriceAsync("btc");

            // Arrange
            result.Should().Be(expectedPrice);
        }

        [Fact]
        public async Task GetPrice_UnsuccessfulResult()
        {
            // Arrange
            var randomizer = new Randomizer();
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            string[] responseString =
            [
                "{\"status\":{\"timestamp\":\"2018-06-02T22:51:28.209Z\",\"error_code\":1002,\"error_message\":\"API key missing.\",\"elapsed\":10,\"credit_count\":0}}"
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
                CoinMarketCap = randomizer.String2(36, 36)
            };
            var coinMarketCapPriceRepository =
                new CoinMarketCapCryptoPriceRepository(new OptionsWrapper<ApiKeySetting>(apiKeySetting), httpClient);

            // Assert
            Action action = () => _ = coinMarketCapPriceRepository.GetPriceAsync("btc").Result;

            // Arrange
            action.Should().Throw<HttpRequestException>()
                .Where(e => e.StatusCode == HttpStatusCode.Unauthorized);
        }
    }
}
