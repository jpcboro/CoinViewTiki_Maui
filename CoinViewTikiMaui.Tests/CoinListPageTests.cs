using AutoFixture;
using CoinViewTikiMaui.Models;
using CoinViewTikiMaui.Services;
using CoinViewTikiMaui.ViewModels;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinViewTikiMaui.Tests
{
    public class CoinListPageTests
    {
        CoinListPageViewModel coinListPageViewModel;
        
        [Fact]
        public void MustBeTrue()
        {
            Assert.True(true);
        }

        [Fact]
        public async void GetCoinList_IsNotNull()
        {
            //Arrange
            Fixture fixture = new Fixture();
            var mockCoinsList = fixture.CreateMany<MarketUSDCoin>(250).ToList();
            var service = Substitute.For<ICoinGeckoAPIService>();
            service.GetCoinsViaUSDMarketAsync(Arg.Any<int>(),Arg.Any<bool>()).Returns(mockCoinsList);
            var connectivity = Substitute.For<IConnectivityWrapper>();
            connectivity.HasInternet().Returns(true);
            var dialogService = Substitute.For<IDialogService>();
            coinListPageViewModel = new CoinListPageViewModel(service, connectivity, dialogService);

            //Act
            coinListPageViewModel.InitCommand.Execute(null);

            //Assert
            Assert.NotNull(coinListPageViewModel.Coins);


        }
    }
}
