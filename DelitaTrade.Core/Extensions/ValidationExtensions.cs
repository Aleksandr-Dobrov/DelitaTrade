using DelitaTrade.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.Extensions
{
    public static class ValidationExtensions
    {
        public static bool IsValidLicensePlate(this string licensePlate)
        {
            if (licensePlate.Length == 10)
            {
                return char.IsLetter(licensePlate[0])
                && char.IsLetter(licensePlate[1])
                && licensePlate[2] == ' '
                && char.IsDigit(licensePlate[3])
                && char.IsDigit(licensePlate[4])
                && char.IsDigit(licensePlate[5])
                && char.IsDigit(licensePlate[6])
                && licensePlate[7] == ' '
                && char.IsLetter(licensePlate[8])
                && char.IsLetter(licensePlate[9]);
            }
            else if (licensePlate.Length == 9)
            {
                return char.IsLetter(licensePlate[0])
                && licensePlate[1] == ' '
                && char.IsDigit(licensePlate[2])
                && char.IsDigit(licensePlate[3])
                && char.IsDigit(licensePlate[4])
                && char.IsDigit(licensePlate[5])
                && licensePlate[6] == ' '
                && char.IsLetter(licensePlate[7])
                && char.IsLetter(licensePlate[8]);
            }
            else
            {
                return false;
            }
        }
    }
}
