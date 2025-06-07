namespace DelitaTrade.Core.Exporters.ExcelExporters.ExcelComponents
{
    public class ExcelRow
    {
        private int _rowCoordinate;
        private double _rowHeight;

        public ExcelRow(double rowHeight, int rowCoordinate)
        {            
            var dec = rowHeight - Math.Floor(rowHeight);
            var intPart = Math.Floor(rowHeight);
            double result = 0;
            if (dec < 0.25)
            {
                result = intPart;
            }
            else if (dec < 0.75)
            {
                result = intPart + 0.5;
            }
            else
            {
                result = intPart + 1;
            }
            _rowHeight = result;
            _rowCoordinate = rowCoordinate;
        }

        public double RowHeight => _rowHeight;
        public int RowCoordinate => _rowCoordinate;
    }
}
