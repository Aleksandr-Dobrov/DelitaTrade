﻿using Microsoft.EntityFrameworkCore;
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
    [Index(nameof(Name))]
    public class CompanyObject
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(ValidationConstants.CompanyObjectNameMaxLength)]
        [Column(TypeName = ValidationTypeConstants.NVarchar)]
        public string Name { get; set; } = null!;
        public bool IsBankPay { get; set; }
        public bool IsActive { get; set; } = true;
        public int? AddressId { get; set; }
        [ForeignKey(nameof(AddressId))]
        public Address? Address { get; set; }
        public int TraderId { get; set; }
        [ForeignKey(nameof(TraderId))]
        public virtual required Trader Trader { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public virtual Company Company { get; set; } = null!;
    }
}
