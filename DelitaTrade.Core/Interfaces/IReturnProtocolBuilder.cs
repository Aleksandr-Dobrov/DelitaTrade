using DelitaTrade.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.Interfaces
{
    public interface IReturnProtocolBuilder : IDisposable
    {
        public string ExportedFilePath { get; }

        public IReturnProtocolBuilder InitializedExporter(IExportReturnProtocol report, string filePath, Func<string, bool> messageToCloseAndContinue, IEnumerable<DescriptionCategoryViewModel> descriptionCategoryViewModels);

        public IReturnProtocolBuilder BuildHeather();

        public IReturnProtocolBuilder BuildBody();

        public IReturnProtocolBuilder BuildFooter();

        public void Export();
    }
}
