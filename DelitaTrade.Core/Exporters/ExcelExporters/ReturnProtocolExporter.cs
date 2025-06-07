using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.Interfaces;
using DelitaTrade.Core.Stores;
using DelitaTrade.Core.ViewModels;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Reflection;
using System.Threading.Tasks;

namespace DelitaTrade.Core.Exporters.ExcelExporters
{
    public class ReturnProtocolExporter
    {
        private readonly IConfiguration _configuration;
        private readonly IReturnProtocolBuilderService _returnProtocolBuilderService;
        private IReturnProtocolBuilder? _returnProtocolBuilder;
        private readonly IDescriptionCategoryService _descriptionCategoryService;
        private IEnumerable<DescriptionCategoryViewModel?> _descriptionCategories = new List<DescriptionCategoryViewModel>();

        public ReturnProtocolExporter(IConfiguration configuration, IDescriptionCategoryService descriptionCategoryService, IReturnProtocolBuilderService returnProtocolBuilderService)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _returnProtocolBuilderService = returnProtocolBuilderService;
            _descriptionCategoryService = descriptionCategoryService;
            ExportCompleted += () => { };
            ExportStart += () => { };
            ExportFileCreate += (f) => { };
        }

        public event Action ExportCompleted;
        public event Action ExportStart;
        public event Action<string> ExportFileCreate;

        public async Task ExportReturnProtocol(IExportReturnProtocol returnProtocol, string filePath, Func<string, bool> messageToCloseAndContinue)
        {
            try
            {
                await InitiateBuilder();
                if (_returnProtocolBuilder == null) throw new ArgumentNullException(nameof(IReturnProtocolBuilder));
                ExportStart();
                var t = Task.Run(() =>
                {
                    using (_returnProtocolBuilder)
                    {
                        _returnProtocolBuilder.InitializedExporter(returnProtocol, filePath, messageToCloseAndContinue, _descriptionCategories!)
                                         .BuildHeather()
                                         .BuildBody()
                                         .BuildFooter()
                                         .Export();
                    }
                });
                await RiseEventWhenExportCompleted(t);
            }
            catch
            {
                ExportCompleted();
                throw;
            }
        }
        private async Task RiseEventWhenExportCompleted(Task task)
        {
            await task;
            ExportCompleted?.Invoke();
            ExportFileCreate?.Invoke(_returnProtocolBuilder!.ExportedFilePath);
        }

        private async Task InitiateBuilder()
        {
            _descriptionCategories = await _descriptionCategoryService.GetAllAsync();
            _returnProtocolBuilder = _returnProtocolBuilderService.GetSelectedBuilder();            
        }
    }
}
