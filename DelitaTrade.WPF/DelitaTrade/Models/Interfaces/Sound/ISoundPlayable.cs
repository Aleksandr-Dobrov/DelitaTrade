using DelitaTrade.Sounds;

namespace DelitaTrade.Models.Interfaces.Sound
{
    public interface ISoundPlayable
    {
        public string PlayebleFormats { get; }

        void PlaySound(SoundBase sound);
    }
}
