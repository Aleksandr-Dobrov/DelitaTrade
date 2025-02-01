using DelitaTrade.Models.Configurations;
using DelitaTrade.Sounds;
using System.Configuration;

namespace DelitaTrade.Stores
{
    public class SoundStore
    {
        private readonly Dictionary<SoundEfect, SoundBase> _sounds;

        public SoundStore(Configuration config)
        {
            SoundBase[] sounds = SoundBaseConfiguration.GetAllSounds(config);
            _sounds = new Dictionary<SoundEfect, SoundBase>();
            foreach (var sound in sounds)
            {
                _sounds.Add(sound.Name, sound);
            }
        }

        public SoundBase GetSound(SoundEfect sound)
        {
            if (_sounds.ContainsKey(sound) == false)
            {
                throw new ArgumentNullException("Sound is not exists in sound store");
            }
            return _sounds[sound];
        }
    }
}
