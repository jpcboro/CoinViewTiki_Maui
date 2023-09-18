using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinViewTikiMaui.Services
{
    public interface IConnectivityWrapper
    {
        bool HasInternet();
    }
}
