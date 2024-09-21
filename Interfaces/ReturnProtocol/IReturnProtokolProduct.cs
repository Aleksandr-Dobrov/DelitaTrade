using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Interfaces.ReturnProtocol
{
    public interface IReturnProtokolProduct
    {
        public void AddProduct(IProduct product);
        public void RemoveProduct(IProduct product);
        public void UpdateProduct(IProduct productToUpdate, IProduct updatedProduct);
    }
}
