using DelitaTrade.Sounds;
using System.Configuration;

namespace DelitaTrade.Models.Configurations
{
    public abstract class SoundBaseConfiguration : ConfigurationSection
    {
        public abstract SoundEfect EfectName { get; }
        public abstract bool IsOnValueSound { get; set; }
        public abstract string Source { get; set; }
        public abstract string DefaultSource { get; }

        public static SoundBase[] GetAllSounds(Configuration AppConfig)
        {
            List<SoundFX> sounds = new List<SoundFX>();
            Type type = App.Current.GetType();
            var classes = type.Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(SoundBaseConfiguration)) && !t.IsAbstract)
                .Select(t => (SoundBaseConfiguration)Activator.CreateInstance(t));
            foreach (var clazz in classes)
            {
                string className = clazz.GetType().Name;
                if (AppConfig.Sections[clazz.GetType().Name] is null)
                {
                    AppConfig.Sections.Add(clazz.GetType().Name, clazz);
                }
                var ProgramConfigurationSection = (SoundBaseConfiguration)AppConfig.GetSection(clazz.GetType().Name);
                sounds.Add(new SoundFX(ProgramConfigurationSection));
            }
            return sounds.ToArray();
        }
    }
}
