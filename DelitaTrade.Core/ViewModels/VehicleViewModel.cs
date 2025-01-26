using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.ViewModels
{
    public class VehicleViewModel
    {
        public int Id { get; set; }
        public required string LicensePlate { get; set; }
        public string? Model { get; set; }
    }
}
