using CoinViewTikiMaui;
using CoinViewTikiMaui.Services;
using CoinViewTikiMaui.ViewModels;
using CoinViewTikiMaui.Views;
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

            builder.Services.AddSingleton<CoinListPage>();
            builder.Services.AddTransient<CoinListPageViewModel>();
            builder.Services.AddTransient<CoinDetailPage>();
            builder.Services.AddTransient<CoinDetailPageViewModel>();
            builder.Services.AddTransient<ICoinGeckoAPIService,CoinGeckoAPIService>();
            return builder.Build();
        }
    }
}