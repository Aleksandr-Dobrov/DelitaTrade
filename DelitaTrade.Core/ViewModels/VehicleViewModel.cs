using DelitaTrade.Common.Interfaces;
using DelitaTrade.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.ViewModels
{
    public class VehicleViewModel : IExceptionName, INamed, IIdent
    {
        public int Id { get; set; }
        public required string LicensePlate { get; set; }
        public string? Model { get; set; }
        public string Name => LicensePlate;
        public override string ToString()
        {
            return LicensePlate;
        }
    }
}
