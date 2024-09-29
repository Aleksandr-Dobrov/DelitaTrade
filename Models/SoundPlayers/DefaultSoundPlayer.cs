using DelitaTrade.Models.Configurations;
using DelitaTrade.Models.Interfaces.Sound;
using DelitaTrade.Sounds;
using System.Media;

namespace DelitaTrade.Models.SoundPlayers
{
    public class DefaultSoundPlayer : ISoundPlayable
    {
        private const string _playebleFormat = "Wav File Only (*.wav)|*.wav";
        private SoundPlayer _soundPlayer;
        private SoundBase _loadedSound;

        public string PlayebleFormats => _playebleFormat;

        public void PlaySound(SoundBase sound)
        {
            if (_soundPlayer == null || _soundPlayer.SoundLocation != sound.Source)
            {
                Initialized(sound);
            }
            _soundPlayer.Play();
        }

        private void Initialized(SoundBase sound)
        {
            _soundPlayer = new SoundPlayer(sound.Source);
            _loadedSound = sound;
        }
    }
}
