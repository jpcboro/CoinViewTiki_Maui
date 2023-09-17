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
                });

#if DEBUG
		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<CoinListPage>();
            builder.Services.AddTransient<CoinListPageViewModel>();
            builder.Services.AddTransient<ICoinGeckoAPIService,CoinGeckoAPIService>();
            return builder.Build();
        }
    }
}