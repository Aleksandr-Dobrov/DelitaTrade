using DelitaTrade.Models.Configurations;
using DelitaTrade.Sounds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Models.Interfaces.Sound
{
    public interface ISoundPlayable
    {
        void PlaySound(SoundBase sound);

        public string PlayebleFormats { get; }
    }
}
