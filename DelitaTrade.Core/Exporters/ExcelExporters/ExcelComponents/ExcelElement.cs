using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.Exporters.ExcelExporters.ExcelComponents
{
    public class ExcelElement
    {
        private int _rowZeroCoordinate;
        private int _columnZeroCoordinate;
        private int _height;
        private int _wight;
        private string? _backgroundColor = null;
        private string _name = string.Empty;

        private List<ExcelContent> _contents = new();

        private List<ExcelBorder> _borders = new();

        private List<ExcelPicture> _pictures = new();

        public ExcelElement(int zeroRow, int zeroColumn, int height, int wight, string? name = null, string? backgroundColor = null)
        {
            _rowZeroCoordinate = zeroRow;
            _columnZeroCoordinate = zeroColumn;
            _height = height;
            _wight = wight;
            _backgroundColor = backgroundColor;
            if (string.IsNullOrEmpty(name))
            {
                name = $"Element_{zeroRow}_{zeroColumn}";
            }
            else
            {
                _name = name;
            }

            _backgroundColor = backgroundColor;
        }

        public int RowZeroCoordinate => _rowZeroCoordinate;
        public int ColumnZeroCoordinate => _columnZeroCoordinate;
        public int Height => _height;
        public int Wight => _wight;
        public string Name => _name;
        public string? BackgroundColor => _backgroundColor;

        public void AddContent(ExcelContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content), "Content cannot be null");
            }
            _contents.Add(content);
        }

        public void AddBorder(ExcelBorder border)
        {
            if (border == null)
            {
                throw new ArgumentNullException(nameof(border), "Border cannot be null");
            }
            _borders.Add(border);
        }

        public void AddBorderAround(ExcelAroundBorder border)
        {
            _borders.AddRange(border.Borders);
        }

        public void AddPicture(ExcelPicture picture)
        {
            if (picture == null)
            {
                throw new ArgumentNullException(nameof(picture), "Picture cannot be null");
            }
            _pictures.Add(picture);
        }

        public IEnumerable<ExcelContent> Contents => _contents;
        public IEnumerable<ExcelBorder> Borders => _borders;

        public IEnumerable<ExcelPicture> Pictures => _pictures;
    }
}
