using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoinViewTikiMaui.Models;
using Refit;

namespace CoinViewTikiMaui.Services
{
    [Headers("User-Agent: User Agent")]
    public interface ICoinGeckoAPI
    {
        [Get("/api/v3/coins/list")]
        Task<List<Coin>> GetCoins();

        [Get("/api/v3/coins/markets?vs_currency=usd&per_page=250")]
        Task<List<MarketUSDCoin>> GetCoinsByUSDMarket();

        [Get("/api/v3/coins/{id}")]
        Task<CoinData> GetCoinData(string id);
    }
}
