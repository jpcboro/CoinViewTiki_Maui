using CoinViewTikiMaui.Models;
using CoinViewTikiMaui.Services;
using CoinViewTikiMaui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;
using Refit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinViewTikiMaui.ViewModels
{
    public partial class CoinListPageViewModel : BaseViewModel
    {
        ICoinGeckoAPIService coinGeckoAPIService;
        const string baseUrl = "https://api.coingecko.com";

        [ObservableProperty]
        ObservableRangeCollection<Grouping<string, MarketUSDCoin>> coins = new();

        public CoinListPageViewModel(ICoinGeckoAPIService coinGeckoAPIService)
        {
            Title = "Coins List";

            this.coinGeckoAPIService = coinGeckoAPIService;

            Task.Run(async () => await GetCoinList());
        }

        public async Task GetCoinList(bool isForceRefresh = false)
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                List<MarketUSDCoin> coinsList = await coinGeckoAPIService.GetCoinsViaUSDMarketAsync();

                if (Coins.Count != 0)
                    Coins.Clear();

                var sortedCoins = from item in coinsList
                                  orderby item.Name
                                  group item by item.Name[0].ToString().ToUpperInvariant()
                                  into itemGroup
                                  select new Grouping<string, MarketUSDCoin>(itemGroup.Key, itemGroup);

               
                Coins.ReplaceRange(sortedCoins);

            }
            catch (Exception ex)
            {

                Debug.WriteLine($"Unable to get coins: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        async Task Tap(Coin coin)
        {
            string coinId = coin.Id;
            await Shell.Current.GoToAsync($"{nameof(CoinDetailPage)}?Name={coinId}", true);
        }
    }
}
