using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinViewTikiMaui.Services
{
    public class ConnectivityWrapper : IConnectivityWrapper
    {
        public bool HasInternet() => Connectivity.NetworkAccess == NetworkAccess.Internet;
    }
}
