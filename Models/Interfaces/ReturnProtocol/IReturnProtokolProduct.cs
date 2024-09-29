namespace DelitaTrade.Models.Interfaces.ReturnProtocol
{
    public interface IReturnProtokolProduct
    {
        public void AddProduct(IProduct product);
        public void RemoveProduct(IProduct product);
        public void UpdateProduct(IProduct productToUpdate, IProduct updatedProduct);
    }
}
