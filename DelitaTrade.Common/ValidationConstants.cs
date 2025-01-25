using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Common
{
    public class ValidationConstants
    {
        public const int TraderNameMaxLength = 50;
        public const int TraderPhoneNumberMaxLength = 20;

        public const int ProductNameMaxLength = 150;
        public const int ProductUnitMaxLength = 15;

        public const int ReturnProtocolPayMethodLength = 20;

        public const int ReturnedProductDescriptionMaxLength = 100;
        public const int ReturnedProductBatchMaxLength = 30;

        public const int CompanyObjectNameMaxLength = 100;
        public const int CompanyObjectAddressMaxLength = 150;

        public const int CompanyNameMaxLength = 100;
        public const int CompanyTypeMaxLength = 10;
        public const int CompanyBulstadMaxLength = 15;

        public const int UserNameMaxLength = 100;
        public const int UserPasswordMaxLength = 30;

        public const int UserNameMinLength = 4;
        public const int UserPasswordMinLength = 5;

        public const int TownNameMaxLength = 90;
        public const int StreetNameMaxLength = 100;
        public const int StreetNumberMaxLength = 10;
        public const int GpsCoordinatesMaxLength = 30;
        public const int AddressDescriptionMaxLength = 150;

        public const int InvoiceNumberMaxLength = 12;        
    }
}
