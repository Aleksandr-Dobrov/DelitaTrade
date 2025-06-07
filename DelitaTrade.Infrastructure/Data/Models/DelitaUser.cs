using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static DelitaTrade.Common.ValidationConstants.DelitaUserConstants;

namespace DelitaTrade.Infrastructure.Data.Models
{
    public class DelitaUser : IdentityUser<Guid>
    {
        [PersonalData]
        [MaxLength(NameMaxLength)]
        public string? Name { get; set; }

        [PersonalData]
        [MaxLength(LastNameMaxLength)]
        public string? LastName { get; set; }
    }
}
