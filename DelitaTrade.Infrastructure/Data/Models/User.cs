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
    [Index(nameof(Name),IsUnique = true)]
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        [MaxLength(ValidationConstants.UserNameMaxLength)]
        [Column(TypeName = ValidationTypeConstants.NVarchar)]
        public string Name { get; set; } = null!;
        [Required]
        [MaxLength(ValidationConstants.UserPasswordMaxLength)]
        [Column(TypeName = ValidationTypeConstants.NVarchar)]
        public string HashedPassword { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public ICollection<ReturnProtocol> ReturnProtocols { get; set; } = new List<ReturnProtocol>();
        public ICollection<DayReport> dayReports { get; set; } = new List<DayReport>();
    }
}
