using CoinViewTikiMaui.ViewModels;

namespace CoinViewTikiMaui.Views;

public partial class CoinListPage : ContentPage
{
	public CoinListPage(CoinListPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}