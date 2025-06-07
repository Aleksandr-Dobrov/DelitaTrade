using DelitaTrade.Commands;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DelitaTrade.ViewModels.Controllers
{
    public class DescriptionCategoryManagerController : DescriptionCategoryController
    {
        private readonly IServiceProvider _serviceProvider;
        public DescriptionCategoryManagerController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            CategoriesUpdated += () => { };
            _serviceProvider = serviceProvider;
            AddCommand = new DefaultCommand(AddCategory, 
                () => 
                { 
                    return GetErrors(nameof(CategoryName)) == null && 
                           CategoryName != null && 
                           IsExistCategory(CategoryName) == false; 
                }, 
                this, 
                nameof(CategoryName));
            DeleteCommand = new DefaultCommand(DeleteCategory, 
                () => { return SelectedDescriptionCategory != null && IsHaveReferences == false; }, 
                this, 
                nameof(SelectedDescriptionCategory), nameof(IsHaveReferences));
            UpdateCommand = new DefaultCommand(UpdateCategory, 
                () => 
                { 
                    return SelectedDescriptionCategory != null && 
                           CategoryName != null && 
                           GetErrors(nameof(CategoryName)) == null && 
                           SelectedDescriptionCategory.Name != CategoryName && 
                           IsExistCategory(CategoryName) == false; 
                },
                this,
                nameof(SelectedDescriptionCategory), nameof(CategoryName));
            PropertyChanged += OnViewModelPropertyChange;            
        }

        private string? _categoryName;

        [MinLength(5, ErrorMessage = "Category name must be at least 5 character long.")]
        public string? CategoryName
        {
            get => _categoryName;
            set
            {
                _categoryName = value;
                OnPropertyChange(nameof(CategoryName));
            }
        }

        private bool _isHaveReferences;

        public bool IsHaveReferences
        {
            get => _isHaveReferences;
            set 
            {
                _isHaveReferences = value;
                OnPropertyChange(nameof(IsHaveReferences));
            }
        }

        public event Action CategoriesUpdated;

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }

        public ICommand UpdateCommand { get; }

        private async Task AddCategory()
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.GetService<IDescriptionCategoryService>();
            var result = await service.AddDescriptionCategoryAsync(new DescriptionCategoryViewModel { Name = CategoryName! });
            if (result != null)
            {
                DescriptionCategories.Add(result);
                CategoryName = string.Empty;
            }
        }

        private async Task DeleteCategory()
        {
            if (SelectedDescriptionCategory == null) throw new ArgumentNullException("No description category selected");
            using var scope = _serviceProvider.CreateScope();
            var service = scope.GetService<IDescriptionCategoryService>();
            await service.DeleteCategoryAsync(SelectedDescriptionCategory.Id);
            DescriptionCategories.Remove(SelectedDescriptionCategory);
        }

        private async Task UpdateCategory()
        {
            if (SelectedDescriptionCategory == null || CategoryName == null) throw new ArgumentNullException("No description category selected");
            using var scope = _serviceProvider.CreateScope();
            var service = scope.GetService<IDescriptionCategoryService>();
            await service.UpdateCategoryAsync(new DescriptionCategoryViewModel() 
            {
                Id = SelectedDescriptionCategory.Id,
                Name = CategoryName
            });
            var category = DescriptionCategories.First(d => d.Id == SelectedDescriptionCategory.Id);
            var newCategory = new DescriptionCategoryViewModel
            {
                Id = category.Id,
                Name = CategoryName
            };
            DescriptionCategories.Remove(category);
            DescriptionCategories.Add(newCategory);
            OnPropertyChange(nameof(SelectedDescriptionCategory));
            CategoriesUpdated?.Invoke();
        }


        private void OnViewModelPropertyChange(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedDescriptionCategory) && SelectedDescriptionCategory != null)
            {
                IsHaveReferencesCallBack();
            }
        }

        private async void IsHaveReferencesCallBack() 
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.GetService<IDescriptionCategoryService>();
            IsHaveReferences = await service.IsHaveReferences(SelectedDescriptionCategory!.Id);
        }

        private bool IsExistCategory(string name)
        {
            return DescriptionCategories.Any(d => d.Name == name);
        }
    }
}
