using Akavache;
using CoinViewTikiMaui.Constants;
using CoinViewTikiMaui.Models;
using Polly;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinViewTikiMaui.Services
{
    public class CoinGeckoAPIService : ICoinGeckoAPIService
    {
        readonly ICoinGeckoAPI coinGeckoApi;
        const string baseUrl = "https://api.coingecko.com/";

        public CoinGeckoAPIService()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "User Agent");
            httpClient.BaseAddress = new Uri(baseUrl);

            this.coinGeckoApi = RestService.For<ICoinGeckoAPI>(httpClient);
        }

        public async Task<List<MarketUSDCoin>> GetCoinsViaUSDMarketAsync(int days = 1, bool forceRefresh = false)
        {
            if (forceRefresh)
                await BlobCache.LocalMachine.InvalidateObject<List<MarketUSDCoin>>(CacheConstants.AllCoinsUSDList);

            var coinsFromCache = new List<MarketUSDCoin>();

            try
            {
                coinsFromCache = await BlobCache.LocalMachine
                   .GetAndFetchLatest<List<MarketUSDCoin>>(key: CacheConstants.AllCoinsUSDList,
                       GetAndSaveCoins, fetchPredicate: offset =>
                       {
                           var elapsed = DateTimeOffset.Now - offset;
                           return elapsed > new TimeSpan(days: days,
                                hours: 0,
                                minutes: 0,
                                seconds: 0);

                       });

                if (coinsFromCache != null)
                    return coinsFromCache;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to get data from server: {ex}");

                throw;
            }

            return await GetAndSaveCoins();
        }

        private async Task<List<MarketUSDCoin>> GetAndSaveCoins()
        {
            try
            {
                List<MarketUSDCoin> coins = await Policy
                    .Handle<HttpRequestException>(exception =>
                    {
                        Console.WriteLine($"API Exception when connection to Coin Gecko API: {exception.Message}");
                        return true;
                    })
                    .WaitAndRetryAsync(
                        retryCount: 3,
                        sleepDurationProvider: retryAttempt =>
                            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (ex, time) =>
                        {
                            Console.WriteLine($"Retry exception: {ex.Message}, retrying...");
                        }
                    )
                    .ExecuteAsync(async () => await coinGeckoApi.GetCoinsByUSDMarket());


                await BlobCache.LocalMachine.InsertObject(CacheConstants.AllCoinsUSDList,
                    coins, DateTimeOffset.Now.AddSeconds(80));

                return coins;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to get data from server: {ex.Message}");
                throw;
            }
        }

        public async Task<CoinData> GetCoinDetailAsync(string coinId)
        {
            try
            {
                CoinData coinDetail = await Policy.Handle<HttpRequestException>(exception =>
                {
                    Console.WriteLine($"API Exception when connection to Coin Gecko API: {exception.Message}");
                    return true;
                })
                    .WaitAndRetryAsync(retryCount: 3,
                        sleepDurationProvider: retryAttempt =>
                            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (ex, time) =>
                        {
                            Console.WriteLine($"Retry exception: {ex.Message}, retrying...");
                        })
                    .ExecuteAsync(async () => await coinGeckoApi.GetCoinData(coinId));

                return coinDetail;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to get data from server: {ex.Message}");
                throw;
            }
        }
    }
}
