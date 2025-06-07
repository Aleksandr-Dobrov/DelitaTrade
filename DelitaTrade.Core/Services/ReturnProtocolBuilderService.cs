using DelitaTrade.Core.ConfigurationModels;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.Exporters.ExcelExporters;
using DelitaTrade.Core.Interfaces;
using DelitaTrade.Core.ViewModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.Services
{
    public class ReturnProtocolBuilderService : IReturnProtocolBuilderService
    {
        private readonly IReturnProtocolBuildersStore _returnProtocolBuildersStore;
        private readonly Configuration _configuration;
        private IReturnProtocolBuilder? _selectedBuilder;
        
        public ReturnProtocolBuilderService(IReturnProtocolBuildersStore returnProtocolBuildersStore, Configuration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _returnProtocolBuildersStore = returnProtocolBuildersStore ?? throw new ArgumentNullException(nameof(returnProtocolBuildersStore));
            LoadConfigurations();
        }
        public IEnumerable<string> GetAll()
        {
            return _returnProtocolBuildersStore.GetBuilders()
                .Select(b => b.GetType().Name);
        }

        public IReturnProtocolBuilder GetSelectedBuilder()
        {
            return _selectedBuilder ?? throw new InvalidOperationException("No builder has been selected. Please select a builder first.");
        }

        public void SelectBuilder(string name)
        {
            _selectedBuilder = _returnProtocolBuildersStore.GetBuilderByName(name)
                ?? throw new ArgumentException($"No builder found with name {name}", nameof(name));
            var section = _configuration.GetSection(nameof(ReturnProtocolBuilderConfiguration)) as ReturnProtocolBuilderConfiguration
                ?? throw new ArgumentNullException($"{nameof(ReturnProtocolBuilderConfiguration)} is missing");
            section.Builder = name;
            section.CurrentConfiguration.Save();
        }

        private void LoadConfigurations()
        {
            if (_configuration.Sections[nameof(ReturnProtocolBuilderConfiguration)] is null)
            {
                _configuration.Sections.Add(nameof(ReturnProtocolBuilderConfiguration), new ReturnProtocolBuilderConfiguration());
            }
            var section = _configuration.Sections[nameof(ReturnProtocolBuilderConfiguration)] as ReturnProtocolBuilderConfiguration 
                ?? throw new ArgumentNullException($"{nameof(ReturnProtocolBuilderConfiguration)} is missing");

            _selectedBuilder = _returnProtocolBuildersStore.GetBuilderByName(section.Builder)!;
        }
    }
}
