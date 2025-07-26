using DelitaTrade.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.ViewModels.Controllers
{
    public class ReturnProtocolBuildersController : ViewModelBase
    {
        private readonly IReturnProtocolBuilderService _service;
        public ReturnProtocolBuildersController(IReturnProtocolBuilderService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            ReturnProtocolBuilders = [.. _service.GetAll()];
        }
        public ObservableCollection<string> ReturnProtocolBuilders { get; private set; }

        public string SelectedBuilderName
        {
            get => _service.GetSelectedBuilder().GetType().Name;
            set
            {
                if (ReturnProtocolBuilders.Contains(value))
                {
                    _service.SelectBuilder(value);
                    OnPropertyChange(nameof(SelectedBuilderName));
                }
                else
                {
                    throw new ArgumentException($"No builder found with name {value}", nameof(value));
                }
            }
        }

    }
}
