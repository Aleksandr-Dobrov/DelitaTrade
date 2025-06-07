using DelitaTrade.Components.ComponentsViewModel.ErrorComponents;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.ViewModels.Controllers
{
    public class DescriptionCategoryController : ValidationViewModel
    {
        private readonly IServiceProvider _serviceProvider;
        private ObservableCollection<DescriptionCategoryViewModel> _descriptionCategories = new();
        private DescriptionCategoryViewModel? _selectedDescriptionCategory = null;

        public DescriptionCategoryController(IServiceProvider serviceProvider)
        {
            LoadComplete += () => { };
            _serviceProvider = serviceProvider;
            InitializedCallBack();
        }

        public ObservableCollection<DescriptionCategoryViewModel> DescriptionCategories => _descriptionCategories;

        public DescriptionCategoryViewModel? SelectedDescriptionCategory
        {
            get => _selectedDescriptionCategory;
            set
            {
                if (value == null)
                {
                    AddError(nameof(SelectedDescriptionCategory), "Please select a description category");
                }
                else
                {
                    ClearErrors(nameof(SelectedDescriptionCategory));
                }
                _selectedDescriptionCategory = value;
                OnPropertyChange(nameof(SelectedDescriptionCategory));
                OnPropertyChange(nameof(IsSelected));
            }
        }
        
        public bool IsSelected => _selectedDescriptionCategory != null;

        public event Action LoadComplete;

        private async Task InitializedList()
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.GetService<IDescriptionCategoryService>();
            var result = await service.GetAllAsync();
            foreach (var item in result)
            {
                _descriptionCategories.Add(item);
            }
            LoadComplete();
        }

        private async void InitializedCallBack()
        {
            await InitializedList();
        }

        public async void UpdateDescriptionCategories()
        {
            _descriptionCategories.Clear();
            await InitializedList();
        }
    }
}
