using System.Runtime.Serialization;

namespace DelitaTrade.Models.ReturnProtocol
{
    [DataContract]
    public class Product
    {
        [DataMember]
        private string _itemName;
        [DataMember]
        private double _quantity;
        [DataMember]
        private string _unit;
        [DataMember]
        private string _batch;
        [DataMember]
        private string _bestBefore;
        [DataMember]
        private string _description;

        public Product(string itemName, double quantity, string unit, string batch, string bestBefore)
        {
            _itemName = itemName;
            _quantity = quantity;
            _unit = unit;
            _batch = batch;
            _bestBefore = bestBefore;
        }

        public Product(string itemName, double quantity, string unit, string batch, string bestBefore, string description) 
                : this(itemName, quantity, unit, batch, bestBefore)
        {
            _description = description;
        }

        public string ItemName => _itemName;
        public string Unit => _unit;

        public override int GetHashCode()
        {
            return $"{_itemName}{_unit}".GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            var product = obj as Product;
            return product?.ItemName == _itemName && product.Unit == _unit;
        }
    }
}
