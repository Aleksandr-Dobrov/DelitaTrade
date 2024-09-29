using DelitaTrade.Commands;
using DelitaTrade.Components.ComponetsViewModel.OptionsComponentViewModels;
using DelitaTrade.Models;
using DelitaTrade.Models.Configurations;
using DelitaTrade.Models.DataProviders;
using System.IO;

namespace DelitaTrade.Components.ComponentsCommands.SoundCommands
{
    public class SetSoureCommand : CommandBase
    {
        private SoundEfect _sound;
        private string _propertyName;
        private readonly DelitaTradeDayReport _delitaTradeDayReport;
        private readonly SoundOptionsViewModel _soundOptionsViewModel;

        public SetSoureCommand(DelitaTradeDayReport delitaTradeDayReport, SoundEfect sound, string propertyName, SoundOptionsViewModel soundOptionsViewModel)
        {
            _delitaTradeDayReport = delitaTradeDayReport;
            _soundOptionsViewModel = soundOptionsViewModel;
            _sound = sound;
        }

        public override void Execute(object? parameter)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = "wav";
            dialog.Filter = _delitaTradeDayReport.DelitaSoundService.SoundPlayer.PlayebleFormats;            
            bool? result = dialog.ShowDialog();
            if (result == true) 
            {
                FileInfo fileInfo = new FileInfo(dialog.FileName);                
                _delitaTradeDayReport.DelitaSoundService.Configurate(new SoudFXConfigurationProvider(_sound, GetCurentIsOn(_sound), dialog.FileName));
                _soundOptionsViewModel.OnSoursePropertyChange(_propertyName);
            }
        }

        private bool GetCurentIsOn(SoundEfect sound)
        {
            return _delitaTradeDayReport.DelitaSoundService.GetIsOnValue(sound);
        }
    }
}
