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
        ICoinGeckoAPIService coinGeckoAPIService;
        IConnectivityWrapper connectivity;
        IDialogService dialogService;

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

        public CoinDetailPageViewModel(ICoinGeckoAPIService coinGeckoAPIService, 
                                       IConnectivityWrapper connectivity,
                                       IDialogService dialogService)
        {
            this.coinGeckoAPIService = coinGeckoAPIService;
            this.connectivity = connectivity;
            this.dialogService = dialogService;
        }

         public async Task GetCoinDetailAsync(string coinName)
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
                CoinDetails = await this.coinGeckoAPIService.GetCoinDetailAsync(coinName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to get coin details: ", ex.Message);
                await dialogService.ShowOKDialog("Error", $"Something went wrong: {ex.Message}", "OK");

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
