using Microsoft.Office.Interop.Excel;

namespace DelitaTrade.Core.Exporters.ExcelExporters.ExcelComponents
{
    public class BaseExcelComponent
    {
        private int _rowZeroCoordinate;
        private int _columnZeroCoordinate;

        private int _columnsCount;
        private int _rowsCount;

        private ExcelWriter _excelWriter = new ExcelWriter();
        private ExcelDrawer _excelDrawing = new ExcelDrawer();

        private List<ExcelRow> _rows;

        private List<ExcelElement> _elements = new();

        public BaseExcelComponent(int zeroRow, int zeroColumn, int columnCount, params double[] rowsHeight)
        {
            _columnZeroCoordinate = zeroColumn;
            _rowZeroCoordinate = zeroRow;
            _columnsCount = columnCount;
            _rowsCount = rowsHeight.Length;
            _rows = new List<ExcelRow>();

            for (int i = 0; i < rowsHeight.Length; i++)
            {
                _rows.Add(new ExcelRow(rowsHeight[i], zeroRow + i));
            }
        }

        public int RowsCount => _rowsCount;

        public IEnumerable<ExcelElement> Elements => _elements;

        public void SetRowHeight(Worksheet ws)
        {
            for (int i = 0; i < _rows.Count; i++)
            {
                _excelWriter.SetRowHeight(ws, _rows[i].RowCoordinate, _rows[i].RowHeight);
            }
        }

        public void WriteContent(Worksheet ws)
        {            
            WriteAll(ws);
            SetRowHeight(ws);
        }

        public void AddElement(ExcelElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element), "Element cannot be null");
            }
            if (element.RowZeroCoordinate + element.Height - 1 > _rowsCount || element.ColumnZeroCoordinate + element.Wight - 1 > _columnsCount)
            {
                throw new ArgumentOutOfRangeException(nameof(element), "Element exceeds the bounds of the component");
            }
            _elements.Add(element);
        }

        private void WriteAll(Worksheet ws)
        {
            foreach (var element in _elements)
            {
                foreach (var content in element.Contents)
                {
                    _excelWriter.WriteDataToRange(ws, content.Content, content.IsBold, content.FontSize, content.IsDate,
                    content.HorizontalAlign, content.VerticalAlign,
                    _rowZeroCoordinate + element.RowZeroCoordinate - 1,
                    _columnZeroCoordinate + element.ColumnZeroCoordinate - 1,
                    (_rowZeroCoordinate + element.RowZeroCoordinate - 1) + element.Height - 1,
                    (_columnZeroCoordinate + element.ColumnZeroCoordinate - 1) + element.Wight - 1,
                    content.FontName, content.FontColor, content.IsWrapText, content.IsItalic, content.Orientation);
                }

                foreach (var border in element.Borders)
                {
                    _excelDrawing.BorderDraw(ws,
                        _rowZeroCoordinate + element.RowZeroCoordinate - 1,
                        _columnZeroCoordinate + element.ColumnZeroCoordinate - 1,
                        (_rowZeroCoordinate + element.RowZeroCoordinate - 1) + element.Height - 1,
                        (_columnZeroCoordinate + element.ColumnZeroCoordinate - 1) + element.Wight - 1,
                        border.LineStyle, border.BorderWeight, border.BordersIndex, border.HexColor);
                }

                foreach (var picture in element.Pictures)
                {
                    _excelDrawing.DrawPictureToRange(ws,
                        _rowZeroCoordinate + element.RowZeroCoordinate - 1,
                        _columnZeroCoordinate + element.ColumnZeroCoordinate - 1,
                        (_rowZeroCoordinate + element.RowZeroCoordinate - 1) + element.Height - 1,
                        (_columnZeroCoordinate + element.ColumnZeroCoordinate - 1) + element.Wight - 1,
                        picture.ImageWidth, picture.ImageHeight, picture.Path, picture.LeftOfSet, picture.TopOfSet);
                }

                if (!string.IsNullOrEmpty(element.BackgroundColor))
                {
                    string hexColor = element.BackgroundColor;
                    _excelDrawing.BackgroundColorRange(ws, hexColor,
                        _rowZeroCoordinate + element.RowZeroCoordinate - 1,
                        _columnZeroCoordinate + element.ColumnZeroCoordinate - 1,
                        (_rowZeroCoordinate + element.RowZeroCoordinate - 1) + element.Height - 1,
                        (_columnZeroCoordinate + element.ColumnZeroCoordinate - 1) + element.Wight - 1);
                }
            }
            if (_elements.Count == 0)
            {
                _excelWriter.WriteDataToRange(ws, "", false, 10, false,
                    XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter,
                    _rowZeroCoordinate, _columnZeroCoordinate,
                    _rowZeroCoordinate + _rowsCount - 1, _columnZeroCoordinate + _columnsCount - 1);
            }
        }
    }
}
