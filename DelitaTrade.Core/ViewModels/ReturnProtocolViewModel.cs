﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DelitaTrade.Core.ViewModels;

namespace DelitaTrade.Core.ViewModels
{
    public class ReturnProtocolViewModel
    {
        public int Id { get; set; }
        public DateTime ReturnedDate { get; set; }
        public required string PayMethod { get; set; }
        public required CompanyObjectViewModel CompanyObject { get; set; }
        public required TraderViewModel Trader { get; set; }
        public required UserViewModel User { get; set; }
        public ObservableCollection<ReturnedProductViewModel> Products { get; set; } = new ObservableCollection<ReturnedProductViewModel>();

        public override string ToString()
        {
            return $"{CompanyObject.Name} - {ReturnedDate:dd-MM-yy} - {Trader.Name}";
        }
    }
}
