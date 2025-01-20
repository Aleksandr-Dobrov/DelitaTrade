using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.ViewModels
{
    public class AddressViewModel
    {
        public int Id { get; set; }
        public required string Town {  get; set; }
        public string? StreetName { get; set; }
        public string? Number {  get; set; }
        public string? GpsCoordinates { get; set; }
        public string? Description { get; set; }

        public override string ToString()
        {
            return AdvanceJoin(' ', Town, StreetName, Number, Description);
        }

        private string AdvanceJoin(char separator, params string[] args)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < args.Length; i++)
            {
                if (i == 0)
                {
                    stringBuilder.Append(args[i]);
                }
                else
                {
                    if (args[i].IsNullOrEmpty() == false)
                    {
                        stringBuilder.Append(separator);
                        stringBuilder.Append(args[i]);
                    }
                }
            }
            return stringBuilder.ToString();
        }
    }
}
