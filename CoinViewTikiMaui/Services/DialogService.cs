using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinViewTikiMaui.Services
{
    public class DialogService : IDialogService
    {
        public async Task ShowOKDialog(string title, string message, string accept)
        {
          await Shell.Current.DisplayAlert(title, message, accept);
        }
    }
}
