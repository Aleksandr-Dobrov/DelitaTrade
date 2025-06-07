using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.Exporters.ExcelExporters
{
    public class ExcelPageModel
    {
        private int _pageNumber;
        private int _rowsCount;
        private int _startRow;

        public ExcelPageModel(int pageNumber, int rowsCount, int startRow)
        {
            _pageNumber = pageNumber;
            _rowsCount = rowsCount;
            _startRow = startRow;
        }

        public int PageNumber => _pageNumber;
        public int RowsCount => _rowsCount;
        public int StartRow => _startRow;
    }
}
