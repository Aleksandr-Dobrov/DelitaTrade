using DelitaTrade.Core.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;

namespace DelitaTrade.Core.ModelBinders
{
    public class ApproveProductsModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var model = new ReturnProtocolApproveModel();            
            var data = bindingContext.HttpContext.Request.Form;

            var lastChangeSuccess = data.TryGetValue(nameof(DetailReturnProtocolViewModel.LastChange), out StringValues lastChangeValue);
            if (lastChangeSuccess && DateTime.TryParse(lastChangeValue, out DateTime lastChangeDateTime))
            {
                model.LastChange = lastChangeDateTime;
            }

            var productIdsResult = data.TryGetValue("ReturnedProductIds", out StringValues productIds);
            List<string> productIdsCollection = new();
            if (productIdsResult && productIds.Count > 0 && string.IsNullOrEmpty(productIds[0]) == false)
            {
                productIdsCollection = productIds[0]!
                    .Split(',')
                    .Where(p => !string.IsNullOrEmpty(p))
                    .ToList();
            }

            var returnedProducts = new List<ReturnedProductApproveModel>();
            foreach ( var productId in productIdsCollection)
            {
                if (int.TryParse(productId, out int id) 
                    && data.TryGetValue($"ReturnedProducts[{id}].IsScrapped", out StringValues isScraped)
                    && isScraped.Count > 0
                    && bool.TryParse(isScraped[0], out bool isScrapedValue))
                {
                    
                    returnedProducts.Add(new ReturnedProductApproveModel
                    {
                        Id = id,
                        IsScraped = isScrapedValue,
                        WarehouseDescription = data.TryGetValue($"ReturnedProducts[{id}].WarehouseDescription", out StringValues warehouseDescription) 
                            ? warehouseDescription.ToString() 
                            : null
                    });
                }
            }
            model.ReturnedProducts = returnedProducts;
            model.Id = int.TryParse(data["Id"], out int protocolId) ? protocolId : 0;

            bindingContext.Result = ModelBindingResult.Success(model);

            return Task.CompletedTask;
        }
    }
}
