using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinViewTikiMaui.Models
{
    public class MarketUSDCoin : Coin
    {
        [JsonProperty(PropertyName = "image")]
        public string Image { get; set; }
    }
}
