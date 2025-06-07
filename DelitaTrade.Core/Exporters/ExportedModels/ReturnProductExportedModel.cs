using DelitaTrade.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.Exporters.ExportedModels
{
    public class ReturnProductExportedModel : IExportedReturnedProduct
    {
        public ReturnProductExportedModel(string name, double quantity, string unit, string batch, string bestBefore, string description, string descriptionCategory)
        {
            Name = name;
            Quantity = quantity;
            Unit = unit;
            Batch = batch;
            BestBefore = bestBefore;
            Description = description;
            DescriptionCategory = descriptionCategory;
        }

        public string Name { get; private set; }

        public double Quantity { get; private set; }

        public string Unit { get; private set; }

        public string Batch { get; private set; }

        public string BestBefore { get; private set; }

        public string Description { get; private set; }

        public string DescriptionCategory { get; private set; }
    }
}
