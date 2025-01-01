using DelitaTrade.Models.Interfaces.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Models.ReturnProtocolSQL
{
    public class SqlProduct : IDBData
    {
        public int Id { get; } = -1;
        public string? Name { get; set; }
        public string? Unit {  get; set; }

        public SqlProduct(string name, string unit)
        {
            Name = name;
            Unit = unit;
        }

        public SqlProduct(int id, string name, string unit) : this(name, unit)
        {
            Id = id;
        }
        public string Parameters => "name-=-unit";

        public string Data => $"{Name}-=-{Unit}";

        public string Procedure => "add_product";

        public int NumberOfAdditionalParameters => 0;
    }
}
