using DelitaTrade.Interfaces.ReturnProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Models.ReturnProtocol
{
    public class ReturnProtocolProductDataService : IReturnProtokolProduct
    {
        private ReturnProtocolDelita _returnProtocol;

        public ReturnProtocolProductDataService(ReturnProtocolDelita returnProtocolDelita)
        {
            _returnProtocol = returnProtocolDelita;
        }
        
        public void AddProduct(IProduct product)
        {
            if (ProductCastValidation(product))
            {
                _returnProtocol.AddProduct(product as Product);
            }
        }

        public void RemoveProduct(IProduct product)
        {
            if (ProductCastValidation(product))
            {
                _returnProtocol.RemoveProduct(product as Product);
            }
        }

        public void UpdateProduct(IProduct productToUpdate, IProduct updatedProduct)
        {
            if (ProductCastValidation(productToUpdate, updatedProduct))
            {
                _returnProtocol.UpdateProduct(productToUpdate as Product, updatedProduct as Product);
            }
        }

        private bool ProductCastValidation(IProduct product)
        {
            if (product != null && product is Product)
            {
                return true;
            }
            else if (product == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                throw new ArgumentException("Can not cast IProduct to Product");
            }
        }

        private bool ProductCastValidation(IProduct product1, IProduct product2)
        {            
            return ProductCastValidation(product1) && ProductCastValidation(product2);            
        }
    }
}
