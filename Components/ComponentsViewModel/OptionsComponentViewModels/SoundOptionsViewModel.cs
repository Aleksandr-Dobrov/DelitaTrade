using DelitaTrade.Models;
using DelitaTrade.Components.ComponentsCommands.SoundCommands;
using DelitaTrade.Models.Configurations;
using DelitaTrade.Models.DataProviders;
using System.Windows.Input;
using DelitaTrade.ViewModels;
using DelitaTrade.Services;

namespace DelitaTrade.Components.ComponentsViewModel.OptionsComponentViewModels
{
    public class SoundOptionsViewModel : ViewModelBase
    {
        private readonly DelitaSoundService _delitaSoundService;
        public SoundOptionsViewModel(DelitaSoundService delitaSoundService)
        {
            _delitaSoundService = delitaSoundService;
            SetCashSource = new SetSoureCommand(delitaSoundService, SoundEfect.Cash, nameof(CashSource), this);           
            SetAddInvoiceSource = new SetSoureCommand(delitaSoundService, SoundEfect.AddInvoice, nameof(AddInvoiceSource), this);
            SetRemoveInvoiceSource = new SetSoureCommand(delitaSoundService, SoundEfect.DeleteInvoice, nameof(RemoveInvoiceSource), this);
        }

        public bool CashSoundIsOn 
        {
            get => _delitaSoundService.GetIsOnValue(SoundEfect.Cash);
            set 
            {                
                _delitaSoundService.Configurate(
                    new SoudFXConfigurationProvider(SoundEfect.Cash, value, GetCurrentSource(SoundEfect.Cash)));
                OnPropertyChange();
            }
        }

        public bool AddInvoiceSoundIsOn
        {
            get => _delitaSoundService.GetIsOnValue(SoundEfect.AddInvoice);
            set
            {
                _delitaSoundService.Configurate(
                    new SoudFXConfigurationProvider(SoundEfect.AddInvoice, value, GetCurrentSource(SoundEfect.AddInvoice)));
                OnPropertyChange();
            }
        }
        public bool DeleteInvoiceSoundIsOn
        {
            get => _delitaSoundService.GetIsOnValue(SoundEfect.DeleteInvoice);            
            set
            {
                _delitaSoundService.Configurate(
                    new SoudFXConfigurationProvider(SoundEfect.DeleteInvoice, value, GetCurrentSource(SoundEfect.DeleteInvoice)));
                OnPropertyChange();
            }
        }

        public void OnSoursePropertyChange(string sound)
        {
            OnPropertyChange(sound);           
        }

        public string CashSource
        {
            get => GetFileName(_delitaSoundService.GetSource(SoundEfect.Cash));
        }

        public string AddInvoiceSource
        {
            get => GetFileName(_delitaSoundService.GetSource(SoundEfect.AddInvoice));
        }

        public string RemoveInvoiceSource
        {
            get=> GetFileName(_delitaSoundService.GetSource(SoundEfect.DeleteInvoice));            
        }

        public ICommand SetCashSource { get; }
        public ICommand SetAddInvoiceSource { get; }
        public ICommand SetRemoveInvoiceSource { get; }

        private string GetFileName(string filePath)
        {
            string sourse = filePath;
            int index = 0;
            if (sourse.LastIndexOf("/") > 0)
            {
                index = sourse.LastIndexOf("/");
            }
            else if (sourse.LastIndexOf("\\") > 0)
            {
                index = sourse.LastIndexOf("\\");
            }
            return sourse.Substring(index + 1);
        }

        private string GetCurrentSource(SoundEfect sound)
        {
            return _delitaSoundService.GetSource(sound);
        }
    }
}
