using DelitaTrade.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DelitaTrade.Common.Interfaces;

namespace DelitaTrade.Core.ViewModels
{
    public class TraderViewModel : IExceptionName, INamed, IIdent
    {
        public int Id { get; set; } = -1;
        [MinLength(3)]
        public required string Name { get; set; }
        public string? PhoneNumber { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
