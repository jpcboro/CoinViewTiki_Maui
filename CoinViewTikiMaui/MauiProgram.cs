using CoinViewTikiMaui;
using CoinViewTikiMaui.Services;
using CoinViewTikiMaui.ViewModels;
using CoinViewTikiMaui.Views;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
namespace CoinViewTikiMaui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("proxima_nova_bold", "ProximaNovaBold");
                    fonts.AddFont("proxima_nova_regular", "ProximaNovaReg");
                    fonts.AddFont("proxima_nova_semibold", "ProximaNovaSemiBold");
                });

            Akavache.Registrations.Start("ApplicationName");

#if DEBUG
       builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<ICoinGeckoAPIService, CoinGeckoAPIService>();
            builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
            builder.Services.AddSingleton<IConnectivityWrapper, ConnectivityWrapper>();
            builder.Services.AddSingleton<CoinListPage>();
            builder.Services.AddSingleton<CoinListPageViewModel>();
            builder.Services.AddSingleton<IDialogService, DialogService>();
            builder.Services.AddTransient<CoinDetailPage>();
            builder.Services.AddTransient<CoinDetailPageViewModel>();
            
            return builder.Build();
        }
    }
}