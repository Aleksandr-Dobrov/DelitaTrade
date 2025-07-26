using DelitaTrade.Models.Configurations.SoundConfigurations;
using System.CodeDom;
using System.Configuration;

namespace DelitaTrade.Models.Configurations
{
    public class SoundConstants
    {
        public const string CashEfectName = "cashEfectSound";
        public const string CashIsOnValueName = "cashSound";
        public const string CashSourseName = "cashSource";
        public const string CashDefaultSourseName = "cashDefaultSource";
        public const bool CashDefaultIsOnValue = true;
        public const string CashSourceDefaultValue = "../../../Sounds/PayDesk/427237__get_accel__51-coins.wav";

        public const string AddInvoiseEfectName = "addInvoiceEfectName";
        public const string AddInvoiceName = "addInvoieSound";
        public const string AddInvoiceSourseName = "addInvoiceSource";
        public const string AddInvoicDefaultSourseName = "addInvoiceDefaultSource";
        public const bool AddInvoiceDefaultValue = true;
        public const string AddInvoiceDefaultSource = "../../../Sounds/DayReport/181052__jakobhandersen__pencil_check_mark_1.wav";

        public const string RemoveInvoiceEfectName = "removeInvoiceEfectName";
        public const string RemoveInvoiceName = "deleteInvoiceSound";
        public const string RemoveInvoiceSourceName = "removeInvoiceSourceName";
        public const string RemoveInvoiceDefaulSourceName = "removeInvoiceDefaultSourceName";
        public const bool RemoveInvoiceDefaultValue = true;
        public const string RemoveInvoiceDefaultSource = "../../../Sounds/DayReport/327894__kreastricon62__bush-cut.wav";
    }

    public enum SoundEfect
    {        
        Cash,
        AddInvoice,
        DeleteInvoice
    }
}
