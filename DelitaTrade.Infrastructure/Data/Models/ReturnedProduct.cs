﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DelitaTrade.Infrastructure.Data.Models;
using DelitaTrade.Common;
using Microsoft.EntityFrameworkCore;

namespace DelitaTrade.Infrastructure.Data.Models
{
    public class ReturnedProduct
    {
        [Key]
        public int Id { get; set; }
        public double Quantity { get; set; }
        [Required]
        [MaxLength(ValidationConstants.ReturnedProductBatchMaxLength)]
        [Column(TypeName = ValidationTypeConstants.NVarchar)]
        public string Batch { get; set; } = null!;
        public DateTime BestBefore { get; set; }
        public bool? IsScrapped { get; set; }
        [MaxLength(ValidationConstants.WarehouseDescriptionMaxLength)]
        [Column(TypeName = ValidationTypeConstants.NVarchar)]
        public string? WarehouseDescription { get; set; }
        public virtual Product Product { get; set; } = null!;
        public virtual ReturnedProductDescription? Description { get; set; }
        [DeleteBehavior(DeleteBehavior.Restrict)]
        public virtual DescriptionCategory DescriptionCategory { get; set; } = null!;
        public int ReturnProtocolId { get; set; }
        [ForeignKey(nameof(ReturnProtocolId))]
        public virtual ReturnProtocol ReturnProtocol { get; set; } = null!;
    }
}
