using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.Exporters.ExcelExporters.ExcelComponents
{
    public class ExcelPicture
    {
        private float _imageWidth;
        private float _imageHeight;
        private string _path;
        private float _leftOfSet;
        private float _topOfSet;

        public ExcelPicture(float imageWidth, float imageHeight, string path, float leftOfSet, float topOfSet)
        {
            _imageWidth = imageWidth;
            _imageHeight = imageHeight;
            _path = path;
            _leftOfSet = leftOfSet;
            _topOfSet = topOfSet;
        }

        public float ImageWidth => _imageWidth;
        public float ImageHeight => _imageHeight;
        public string Path => _path;
        public float LeftOfSet => _leftOfSet;
        public float TopOfSet => _topOfSet;
    }
}
