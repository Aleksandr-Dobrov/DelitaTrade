using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static DelitaTrade.Common.DelitaDbConstants;

namespace DelitaTrade.Infrastructure.Data.Models.EntityConfigurations
{
    internal class TraderConfiguration : IEntityTypeConfiguration<Trader>
    {
        public void Configure(EntityTypeBuilder<Trader> builder)
        {
            builder.HasData
            (
                new Trader
                {   
                    Id = DefaultTraderId,
                    Name = DefaultTraderName,
                    IsActive = true                        
                }
            );
        }
    }
}
