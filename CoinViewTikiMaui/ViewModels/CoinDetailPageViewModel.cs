using CoinViewTikiMaui.Models;
using CoinViewTikiMaui.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinViewTikiMaui.ViewModels
{
    [QueryProperty("Name", "Name")]
    public partial class CoinDetailPageViewModel : BaseViewModel
    {
        readonly ICoinGeckoAPIService coinGeckoAPIService;
        string name;
        public string Name
        {
            get => name;
            set
            {
                SetProperty(ref name, value);

                Task.Run(async () => await GetCoinDetailAsync(value));
            }
        }

        [ObservableProperty]
        CoinData coinDetails = new();

        public CoinDetailPageViewModel(ICoinGeckoAPIService coinGeckoAPIService)
        {
            this.coinGeckoAPIService = coinGeckoAPIService;
        }

         public async Task GetCoinDetailAsync(string coinName)
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                CoinDetails = await this.coinGeckoAPIService.GetCoinDetailAsync(coinName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to get coin details: ", ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        async Task GoBack()
        {
            await Shell.Current.GoToAsync("..", true);
        }
    }
}
