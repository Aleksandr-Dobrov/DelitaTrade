using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System.Text.Json;

namespace DelitaTrade.Infrastructure.Data.Models.EntityConfigurations
{
    public class DayReportConfiguration : IEntityTypeConfiguration<DayReport>
    {
        public void Configure(EntityTypeBuilder<DayReport> builder)
        {     
            var dictionaryComparer = new ValueComparer<Dictionary<decimal, int>>(
                (x, y) => x.SequenceEqual(y),
                x => x.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                x => x.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));

            builder.Property(d => d.Banknotes)
                .HasConversion(v => JsonConvert.SerializeObject(v),
                                 v => JsonConvert.DeserializeObject<Dictionary<decimal,int>>(v))
                .Metadata.SetValueComparer(dictionaryComparer);
        }
    }
}
