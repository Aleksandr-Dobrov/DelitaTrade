using DelitaTrade.Commands;
using DelitaTrade.Components.ComponentsViewModel.OptionsComponentViewModels;
using DelitaTrade.Models;
using DelitaTrade.Models.Configurations;
using DelitaTrade.Models.DataProviders;
using DelitaTrade.Services;
using System.IO;

namespace DelitaTrade.Components.ComponentsCommands.SoundCommands
{
    public class SetSoureCommand : CommandBase
    {
        private SoundEfect _sound;
        private string _propertyName;
        private readonly DelitaSoundService _delitaSoundService;
        private readonly SoundOptionsViewModel _soundOptionsViewModel;

        public SetSoureCommand(DelitaSoundService delitaSoundService, SoundEfect sound, string propertyName, SoundOptionsViewModel soundOptionsViewModel)
        {
            _delitaSoundService = delitaSoundService;
            _soundOptionsViewModel = soundOptionsViewModel;
            _sound = sound;
        }

        public override void Execute(object? parameter)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = "wav";
            dialog.Filter = _delitaSoundService.SoundPlayer.PlayebleFormats;            
            bool? result = dialog.ShowDialog();
            if (result == true) 
            {
                FileInfo fileInfo = new FileInfo(dialog.FileName);                
                _delitaSoundService.Configurate(new SoudFXConfigurationProvider(_sound, GetCurrentIsOn(_sound), dialog.FileName));
                _soundOptionsViewModel.OnSoursePropertyChange(_propertyName);
            }
        }

        private bool GetCurrentIsOn(SoundEfect sound)
        {
            return _delitaSoundService.GetIsOnValue(sound);
        }
    }
}
