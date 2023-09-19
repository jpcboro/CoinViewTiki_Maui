using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinViewTikiMaui.Behaviors
{
    public partial class SearchTextChangedBehavior : Behavior<SearchBar>
    {
        protected override void OnAttachedTo(SearchBar bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += Search_TextChanged;
        }

        protected override void OnDetachingFrom(SearchBar bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= Search_TextChanged;
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((SearchBar)sender).SearchCommand?.Execute(e.NewTextValue);
        }
    }
}
