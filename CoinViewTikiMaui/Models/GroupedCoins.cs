using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinViewTikiMaui.Models
{
    public class GroupedCoins : List<Coin>
    {
        public string Name { get; private set; }

        public GroupedCoins(string name, List<Coin> coins) : base(coins)
        {
            Name = name;
        }
    }
}
