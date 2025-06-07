using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.ViewModels
{
    public class DescriptionCategoryViewModel
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
