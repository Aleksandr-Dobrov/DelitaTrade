using DelitaTrade.Models.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.ViewModels
{
    public class InternetProviderViewModel : ViewModelBase
    {
        private readonly InternetProvider _internetProvider;
        private bool _isConnected;
        public InternetProviderViewModel(InternetProvider internetProvider)
        {
            _internetProvider = internetProvider;
            _internetProvider.NetworkStatusChange += OnConnectionChange;
            _isConnected = _internetProvider.CheckForInternetConnection();
        }

        public bool IsConnected => _isConnected;

        private void OnConnectionChange()
        {
            _isConnected = _internetProvider.CheckForInternetConnection();
            OnPropertyChange(nameof(IsConnected));
        }
    }
}
