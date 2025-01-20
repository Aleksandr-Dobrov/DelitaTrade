using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DelitaTrade.Infrastructure.Data.Models;
using DelitaTrade.Common;

namespace DelitaTrade.Infrastructure.Data.Models
{
    [PrimaryKey(nameof(Name), nameof(Unit))]
    public class Product
    {
        [MaxLength(ValidationConstants.ProductNameMaxLength)]
        [Column(TypeName = ValidationTypeConstants.NVarchar)]
        public string Name { get; set; } = null!;

        [MaxLength(ValidationConstants.ProductUnitMaxLength)]
        [Column(TypeName = ValidationTypeConstants.NVarchar)]
        public string Unit { get; set; } = null!;

        public override int GetHashCode()
        {
            return $"{Name}{Unit}".GetHashCode();
        }
        public override bool Equals(object? obj)
        {
            return obj is Product product && product.Name == Name && product.Unit == Unit;
        }
    }
}
