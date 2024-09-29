using DelitaTrade.Models.Configurations;
using DelitaTrade.Models.Interfaces.Configuration;
using DelitaTrade.Models.Interfaces.Sound;
using DelitaTrade.Sounds;
using DelitaTrade.Stores;

namespace DelitaTrade.Services
{
    public class DelitaSoundService
    {
        private readonly SoundStore _soundStore;
        private ISoundPlayable _soundPlayer;

        public DelitaSoundService(ISoundPlayable soundPlayer, SoundStore soundStore)
        {
            _soundStore = soundStore;
            _soundPlayer = soundPlayer;
            ConfigurationChanged += SafeConfiguration;
        }

        public event Action<SoundBase> ConfigurationChanged;

        public ISoundPlayable SoundPlayer => _soundPlayer;

        public bool GetIsOnValue(SoundEfect sound)
        {
            return IsSoundOn(sound);
        }

        public string GetSource(SoundEfect sound)
        {
            return _soundStore.GetSound(sound).Source;
        }

        public void PlaySound(SoundEfect sound)
        {
            if (IsSoundOn(sound))
            {
                _soundPlayer.PlaySound(_soundStore.GetSound(sound));
            }
        }
                
        private bool IsSoundOn(SoundEfect sound)
        {
            return _soundStore.GetSound(sound).IsOn;
        }

        private void SafeConfiguration(SoundBase sound)
        {
            sound.ConfigurationSave();
        }
               
        public void Configurate(IConfigurator provider)
        {            
            var sound = _soundStore.GetSound(provider.Provider.Name) ?? throw new InvalidOperationException("Sound not found");
            sound.Configurate(provider);
            ConfigurationChanged(sound);
        }
    }
}
