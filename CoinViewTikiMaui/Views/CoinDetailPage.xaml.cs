using CoinViewTikiMaui.ViewModels;

namespace CoinViewTikiMaui.Views;

public partial class CoinDetailPage : ContentPage
{
	public CoinDetailPage(CoinDetailPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}