using DelitaTrade.Core.Exporters.ExcelExporters;
using DelitaTrade.Core.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.Stores
{
    public class ReturnProtocolBuildersStore : IReturnProtocolBuildersStore
    {
        private readonly List<IReturnProtocolBuilder> _builders = new List<IReturnProtocolBuilder>();

        public ReturnProtocolBuildersStore(
            ExcelReturnProtocolBuilder excelReturnProtocolBuilder, 
            ExcelCrisisReturnProtocolBuilder excelCrisisReturnProtocolBuilder,
            ExcelStupidReturnProtocolBuilder excelStupidReturnProtocolBuilder,
            ExcelStupidCardReturnProtocolBuilder excelStupidCardReturnProtocolBuilder
            )
        {    
            AddBuilders(
                excelReturnProtocolBuilder,
                excelCrisisReturnProtocolBuilder,
                excelStupidReturnProtocolBuilder,
                excelStupidCardReturnProtocolBuilder
            );
        }

        public IReturnProtocolBuilder? GetBuilderByName(string name)
        {
            return _builders.FirstOrDefault(b => b.GetType().Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<IReturnProtocolBuilder> GetBuilders()
        {
            return _builders.AsReadOnly();
        }
        
        public void AddBuilders(params IReturnProtocolBuilder[] builder)
        {
            _builders.AddRange(builder);
        }
    }
}
