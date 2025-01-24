﻿using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.ViewModels.Interfaces
{
    public interface ICompanyData : INotifyPropertyChanged, IInvokablePropertyChange, IHasError
    {
        string CompanyName { get; }
        string CompanyType { get; }
        string Bulstad {  get; }
    }
}
