using Microsoft.Office.Interop.Excel;

namespace DelitaTrade.Core.Exporters.ExcelExporters.ExcelComponents
{
    public class ExcelContent
    {
        private string _content = string.Empty;
        private int _FontSize;
        private bool _isBold;
        private bool _isItalic;
        private bool _isWrapText;
        private bool _isDate;
        private string _fontName = "Arial";
        private string? _fontColor;
        private XlHAlign _horizontalAlign;
        private XlVAlign _verticalAlign;
        private XlOrientation _orientation;

        public ExcelContent(string content, int fontSize)
        {
            _content = content;
            _FontSize = fontSize;
        }

        public ExcelContent(string content, int fontSize, XlHAlign horizontalAlign = XlHAlign.xlHAlignLeft, XlVAlign verticalAlign = XlVAlign.xlVAlignCenter
            ,bool isBold = false, bool isItalic = false, bool isWrapText = false, bool isDate = false, string fontName = "Arial", string? fontColor = default, XlOrientation orientation = XlOrientation.xlHorizontal) : this(content, fontSize)
        {
            _isBold = isBold;
            _isItalic = isItalic;
            _isWrapText = isWrapText;
            _isDate = isDate;
            _fontName = fontName;
            _fontColor = fontColor;
            _horizontalAlign = horizontalAlign;
            _verticalAlign = verticalAlign;
            _orientation = orientation;
        }

        public string Content => _content;
        public int FontSize => _FontSize;
        public bool IsBold => _isBold;
        public bool IsItalic => _isItalic; 
        public bool IsWrapText => _isWrapText;
        public bool IsDate => _isDate;
        public string FontName => _fontName;
        public string? FontColor => _fontColor;
        public XlHAlign HorizontalAlign => _horizontalAlign;
        public XlVAlign VerticalAlign => _verticalAlign;
        public XlOrientation Orientation => _orientation;
    }
}
