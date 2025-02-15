using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System.Text.Json;

namespace DelitaTrade.Infrastructure.Data.Models.EntityConfigurations
{
    public class DayReportConfiguration : IEntityTypeConfiguration<DayReport>
    {
        public void Configure(EntityTypeBuilder<DayReport> builder)
        {            
            builder.Property(d => d.Banknotes)
                .HasConversion(v => JsonConvert.SerializeObject(v),
                                v => JsonConvert.DeserializeObject<Dictionary<decimal,int>>(v));
        }
    }
}
