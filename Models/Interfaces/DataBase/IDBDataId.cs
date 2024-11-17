using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Models.Interfaces.DataBase
{
    public interface IDBDataId : IDBData
    {
        string DBDataId { get; }
    }
}
