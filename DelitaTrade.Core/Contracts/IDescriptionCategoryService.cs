using DelitaTrade.Core.ViewModels;

namespace DelitaTrade.Core.Contracts
{
    public interface IDescriptionCategoryService
    {
        Task<IEnumerable<DescriptionCategoryViewModel>> GetAllAsync();
        Task<DescriptionCategoryViewModel> GetByIdAsync(int id);
        Task<DescriptionCategoryViewModel> AddDescriptionCategoryAsync(DescriptionCategoryViewModel descriptionCategory);
        Task DeleteCategoryAsync(int id);
        Task UpdateCategoryAsync(DescriptionCategoryViewModel descriptionCategory);
        Task<bool> IsHaveReferences(int id);
    }
}
