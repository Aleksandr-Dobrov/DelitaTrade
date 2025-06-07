using DelitaTrade.Common.Constants;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Data.Models;
using DelitaTrade.Models.JsonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Common
{
    public static class ProductExtension
    {
        public static ProductViewModel Parse(this ProductJsonModel product)
        {
            if (product != null)
            {
                string unit = string.Empty;
                if (product.Unit == "КГ.")
                {
                    unit = ProductUnit.Kg;
                }
                else if (product.Unit == "БР." || product.Unit == "БРОЙ")
                {
                    unit = ProductUnit.Count;
                }
                else if (product.Unit == "КАШОН")
                {
                    unit = ProductUnit.Box;
                }
                else if (product.Unit == "Л.")
                {
                    unit = ProductUnit.Liter;
                }
                else if (product.Unit == "ПАК.")
                {
                    unit = ProductUnit.Pack;
                }
                else if (product.Unit == "М.")
                {
                    unit = ProductUnit.Meter;
                }
                else
                {
                    throw new ArgumentException($"{product.Unit} is incorrect unit value");
                }

                ProductViewModel result = new ProductViewModel()
                {
                    Name = product.Name ?? throw new ArgumentNullException(nameof(product.Name)),
                    Unit = unit,
                    Number = product.Number
                };
                return result;
            }
            throw new ArgumentNullException(nameof(ProductJsonModel));
        }
    }
}
