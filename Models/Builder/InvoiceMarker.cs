namespace DelitaTrade.Models.Builder
{
    public class InvoiceMarker
    {
        private const char _initialMarker = 'A';

        private char _marker = _initialMarker;
        private string _invoiceId;

        public InvoiceMarker(string invoiceId, char marker = _initialMarker)
        {
            _marker = marker;
            _invoiceId = invoiceId;
        }

        public InvoiceMarker(InvoiceMarker invoiceMarker, string invoiceId)
        {
            IncreaseMarker(invoiceMarker);
            _invoiceId = invoiceId;
        }

        public char Marker => _marker;
        public string InvoiceId => _invoiceId;

        public override int GetHashCode()
        {
            return _invoiceId.GetHashCode();
        }

        public override string ToString()
        {
            return $"({_marker})-->";
        }

        private void IncreaseMarker(InvoiceMarker invoiceMarker)
        {
            int index = invoiceMarker.Marker;
            _marker = (char)(index + 1);
        }
    }
}
