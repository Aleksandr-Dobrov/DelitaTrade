using System.Runtime.Serialization;

namespace DelitaTrade.Models.ReturnProtocol
{
    [DataContract]
    public class Product : ProductBase
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

        public Product(string itemName, double quantity, string unit, string batch, string bestBefore) : base(itemName,unit)
        {
            _quantity = quantity;           
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

        public string Batch => _batch;

        public override int GetHashCode()
        {
            return $"{_itemName}{_unit}{_batch}".GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            var product = obj as Product;
            return product?.ItemName == _itemName && product.Unit == _unit && product.Batch == _batch;
        }

        public override string ToString()
        {
            return $"{ItemName}-{Unit}-{Batch}";
        }
    }
}
