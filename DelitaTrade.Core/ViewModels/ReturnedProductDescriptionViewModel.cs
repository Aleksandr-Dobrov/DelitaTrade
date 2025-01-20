using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.ViewModels
{
    public class ReturnedProductDescriptionViewModel
    {
        public ReturnedProductDescriptionViewModel() { }
        public ReturnedProductDescriptionViewModel(string description)
        {
            Description = description;
        }
        public int Id { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return Description;
        }
    }
}
