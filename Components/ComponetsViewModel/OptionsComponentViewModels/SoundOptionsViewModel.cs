using DelitaTrade.Models;
using DelitaTrade.Components.ComponentsCommands.SoundCommands;
using DelitaTrade.Models.Configurations;
using DelitaTrade.Models.DataProviders;
using System.Windows.Input;
using DelitaTrade.ViewModels;

namespace DelitaTrade.Components.ComponetsViewModel.OptionsComponentViewModels
{
    public class SoundOptionsViewModel : ViewModelBase
    {
        private readonly DelitaTradeDayReport _delitaTradeDayReport;
        public SoundOptionsViewModel(DelitaTradeDayReport delitaTradeDayReport)
        {
            _delitaTradeDayReport = delitaTradeDayReport;
            SetCashSource = new SetSoureCommand(delitaTradeDayReport, SoundEfect.Cash, nameof(CashSource), this);           
            SetAddInvoiceSource = new SetSoureCommand(delitaTradeDayReport, SoundEfect.AddInvoice, nameof(AddInvoiceSource), this);
            SetRemoveInvoiceSource = new SetSoureCommand(delitaTradeDayReport, SoundEfect.DeleteInvoice, nameof(RemoveInvoiceSource), this);
        }

        public bool CashSoundIsOn 
        {
            get => _delitaTradeDayReport.DelitaSoundService.GetIsOnValue(SoundEfect.Cash);
            set 
            {                
                _delitaTradeDayReport.DelitaSoundService.Configurate(
                    new SoudFXConfigurationProvider(SoundEfect.Cash, value, GetCurrentSource(SoundEfect.Cash)));
                OnPropertyChange();
            }
        }

        public bool AddInvoiceSoundIsOn
        {
            get => _delitaTradeDayReport.DelitaSoundService.GetIsOnValue(SoundEfect.AddInvoice);
            set
            {
                _delitaTradeDayReport.DelitaSoundService.Configurate(
                    new SoudFXConfigurationProvider(SoundEfect.AddInvoice, value, GetCurrentSource(SoundEfect.AddInvoice)));
                OnPropertyChange();
            }
        }
        public bool DeleteInvoiceSoundIsOn
        {
            get => _delitaTradeDayReport.DelitaSoundService.GetIsOnValue(SoundEfect.DeleteInvoice);            
            set
            {
                _delitaTradeDayReport.DelitaSoundService.Configurate(
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
            get => GetFileName(_delitaTradeDayReport.DelitaSoundService.GetSource(SoundEfect.Cash));
        }

        public string AddInvoiceSource
        {
            get => GetFileName(_delitaTradeDayReport.DelitaSoundService.GetSource(SoundEfect.AddInvoice));
        }

        public string RemoveInvoiceSource
        {
            get=> GetFileName(_delitaTradeDayReport.DelitaSoundService.GetSource(SoundEfect.DeleteInvoice));            
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
            return _delitaTradeDayReport.DelitaSoundService.GetSource(sound);
        }
    }
}
