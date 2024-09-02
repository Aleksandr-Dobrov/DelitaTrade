using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Models.ReturnProtocol
{
    public class ProductNameUnit
    {
        private string _name;
        private string _unit;

        public string Name => _name;
        public string Unit => _unit;

        public override int GetHashCode()
        {
            return $"{_name}{_unit}".GetHashCode();
        }

        public override bool Equals(object? obj)
        {   
            var product = obj as ProductNameUnit;
            return product?.Name == _name && product.Unit == _unit;
        }
    }
}
