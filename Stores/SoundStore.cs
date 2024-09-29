using DelitaTrade.Models.Configurations;
using DelitaTrade.Services;
using DelitaTrade.Sounds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Stores
{
    public class SoundStore
    {
        private readonly Dictionary<SoundEfect, SoundBase> _sounds;

        public SoundStore(SoundBase[] sounds)
        {
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
