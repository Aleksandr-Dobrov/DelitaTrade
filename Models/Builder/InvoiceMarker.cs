using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Models.Builder
{
    public class InvoiceMarker
    {
        private char _marker = 'A';
        private string _invoiceId;

        public InvoiceMarker(string invoiceId, char marker = 'A')
        {
            _marker = marker;
            _invoiceId = invoiceId;
        }

        public InvoiceMarker(InvoiceMarker invoiceMarker)
        {
            IncreaseMarker(invoiceMarker);
        }

        public char Marker => _marker;
        public string InvoiceId => _invoiceId;

        private void IncreaseMarker(InvoiceMarker invoiceMarker)
        {
            int index = invoiceMarker.Marker;
            _marker = (char)(index + 1);
        }
        public override int GetHashCode()
        {
            return _invoiceId.GetHashCode();
        }

        public override string ToString()
        {
            return $"({_marker})-->";
        }
    }
}
