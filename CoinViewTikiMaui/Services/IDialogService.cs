﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinViewTikiMaui.Services
{
    public interface IDialogService
    {
        Task ShowOKDialog(string title, string message, string accept);
    }
}
