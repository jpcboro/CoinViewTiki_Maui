using CoinViewTikiMaui.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
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
            return await coinGeckoApi.GetCoinsByUSDMarket();
        }
    }
}
