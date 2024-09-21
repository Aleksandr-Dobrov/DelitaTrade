using DelitaTrade.Interfaces.ReturnProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Models.ReturnProtocol
{
    [DataContract]
    public class ProductBase : IProduct
    {
        [DataMember]
        private string _itemName;
        [DataMember]
        private string _unit;

        public ProductBase(string itemName, string unit)
        {
            _itemName = itemName;
            _unit = unit;
        }
        public string ItemName => _itemName;

        public string Unit => _unit;

        public override int GetHashCode()
        {
            return $"{_itemName}{_unit}".GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            var product = obj as ProductBase;
            return product?.ItemName == _itemName && product.Unit == _unit;
        }

        public override string ToString()
        {
            return $"{Unit}-{Unit}";
        }
    }
}
