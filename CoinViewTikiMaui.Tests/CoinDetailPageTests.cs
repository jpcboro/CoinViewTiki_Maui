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
    public class CoinDetailPageTests
    {
        CoinDetailPageViewModel coinDetailPageViewModel;

        [Fact]
        public void MustBeTrue()
        {
            Assert.True(true);
        }

        [Fact]
        public async Task GetCoinDetailAsync_Success_IsNotNull()
        {
            //Arrange
            Fixture fixture = new Fixture();
            var mockCoinDetails = fixture.Create<CoinData>();
            var service = Substitute.For<ICoinGeckoAPIService>();
            service.GetCoinDetailAsync(Arg.Any<string>()).Returns(Task.FromResult(mockCoinDetails));
            var connectivity = Substitute.For<IConnectivityWrapper>();
            connectivity.HasInternet().Returns(true);
            var dialogService = Substitute.For<IDialogService>();
            coinDetailPageViewModel = new CoinDetailPageViewModel(service, connectivity, dialogService);
            await coinDetailPageViewModel.GetCoinDetailAsync(mockCoinDetails.Id);

            //Act
            var coinDetails = coinDetailPageViewModel.CoinDetails;
            //Assert
            Assert.NotNull(coinDetails);
            //Assert.Equal(mockCoinDetails.Id, coinDetailPageViewModel.Name);
        }

    }
}
