using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Infrastructure.Data.Models.EntityConfigurations
{
    internal class DescriptionCategoryConfiguration : IEntityTypeConfiguration<DescriptionCategory>
    {
        public void Configure(EntityTypeBuilder<DescriptionCategory> builder)
        {
            builder.HasData
            (
                new DescriptionCategory
                {
                    Id = 1,
                    Name = "Грешна заявка"
                },
                new DescriptionCategory
                {
                    Id = 2,
                    Name = "Отказана поръчка"
                },
                new DescriptionCategory
                {
                    Id = 3,
                    Name = "Ненатоварена стока"
                },
                new DescriptionCategory
                {
                    Id = 4,
                    Name = "Къс срок на годност"
                },
                new DescriptionCategory
                {
                    Id = 5,
                    Name = "Изтегляне на стока"
                },
                new DescriptionCategory
                {
                    Id = 6,
                    Name = "Скъсана опаковка"
                },
                new DescriptionCategory
                {
                    Id = 7,
                    Name = "Развакуумирано"
                }, 
                new DescriptionCategory
                {
                    Id = 8,
                    Name = "Грешно количество"
                }, 
                new DescriptionCategory
                {
                    Id = 9,
                    Name = "Лошо качество"
                }, 
                new DescriptionCategory
                {
                    Id = 10,
                    Name = "Без Етикетировка"
                }
            );
        }
    }
}
