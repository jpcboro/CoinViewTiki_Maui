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
        IConnectivityWrapper connectivity;
        ICoinGeckoAPIService coinGeckoAPIService;
        IDialogService dialogService;
        const string baseUrl = "https://api.coingecko.com";

        [ObservableProperty]
        ObservableRangeCollection<Grouping<string, MarketUSDCoin>> coins = new();

        public CoinListPageViewModel(ICoinGeckoAPIService coinGeckoAPIService, 
                                     IConnectivityWrapper connectivity,
                                     IDialogService dialogService)
        {
            Title = "Coins List";

            this.coinGeckoAPIService = coinGeckoAPIService;
            this.connectivity = connectivity;
            this.dialogService = dialogService;
        }

        public async Task GetCoinList(bool isForceRefresh = false)
        {
            if (IsBusy)
                return;

            try
            {
                if (!connectivity.HasInternet())
                {
                    await dialogService.ShowOKDialog("No connectivity", "Please check internet and try again.", "OK");
                    return;
                }

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
                await dialogService.ShowOKDialog("Error", $"Something went wrong: {ex.Message}", "OK");
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

        [RelayCommand]
        async Task Init()
        {
            await GetCoinList();
        }
    }
}
