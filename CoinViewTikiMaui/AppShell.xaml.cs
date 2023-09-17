using CoinViewTikiMaui.Views;

namespace CoinViewTikiMaui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(CoinDetailPage), typeof(CoinDetailPage));
        }
    }
}