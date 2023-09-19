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
        List<MarketUSDCoin> filteredCoinList = new();
        bool coinsCachedPresent;

        [ObservableProperty]
        ObservableRangeCollection<Grouping<string, MarketUSDCoin>> coins = new();

        [ObservableProperty]
        string searchText;

        [ObservableProperty]
        bool isRefreshing;

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

                List<MarketUSDCoin> coinsList = await coinGeckoAPIService.GetCoinsViaUSDMarketAsync(forceRefresh: isForceRefresh);

                if (Coins.Count != 0)
                    Coins.Clear();

                coinsCachedPresent = coinsList != null;

                if (coinsCachedPresent)
                {
                    coinsCachedPresent = true;
                    var sortedCoins = from item in coinsList
                                      orderby item.Name
                                      group item by item.Name[0].ToString().ToUpperInvariant()
                                      into itemGroup
                                      select new Grouping<string, MarketUSDCoin>(itemGroup.Key, itemGroup);


                    Coins.ReplaceRange(sortedCoins);
                }
                
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

        [RelayCommand]
        async Task Search(string text)
        {
            text = text ?? string.Empty;

            if (text.Length >= 1)
            {
                if (!coinsCachedPresent)
                {
                    await Refresh();
                }
                else
                {
                    await GetCoinList();

                }

                if (Coins.Any())
                {
                    FilterCoinList(text);
                }
                else
                {
                    //offline or an error occured in fetching coins
                    //clear search
                    SearchText = string.Empty;
                }
            }
            else
            {
                //Search Bar is blank or cleared
                //Coins List is set to all Coins
                await GetCoinList();
            }



        }

        private void FilterCoinList(string text)
        {
            var suggestions = Coins.Where(x => x.Items
                .Any(p => p.Name.ToLowerInvariant().StartsWith(text.ToLowerInvariant()))).ToList();


            if (suggestions.Any())
            {
                foreach (var coin in suggestions)
                {
                    filteredCoinList = (from list in coin
                                        where list.Name.ToLowerInvariant().StartsWith(text.ToLowerInvariant())
                                        select list).ToList();
                }

                var newSortedCoins = from item in filteredCoinList
                                     orderby item.Name
                                     group item by item.Name[0].ToString().ToUpperInvariant()
                                     into itemGroup
                                     select new Grouping<string, MarketUSDCoin>(itemGroup.Key, itemGroup);


                Coins.ReplaceRange(newSortedCoins);
            }
            else
            {
                Coins.Clear();
            }
        }

        [RelayCommand]
        async Task Refresh()
        {
            IsRefreshing = true;

            if (connectivity.HasInternet())
            {
                await GetCoinList(isForceRefresh: true);
            }
            else
            {
                await dialogService.ShowOKDialog("No connectivity", "Please check internet and try again.", "OK");

            }

            IsRefreshing = false;
        }
    }
}
