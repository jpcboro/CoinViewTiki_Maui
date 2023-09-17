using CoinViewTikiMaui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinViewTikiMaui.Services
{
    public interface ICoinGeckoAPIService
    {
        Task<List<MarketUSDCoin>> GetCoinsViaUSDMarketAsync(int days = 1, bool forceRefresh = false);
    }
}
