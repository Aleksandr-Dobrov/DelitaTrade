using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;

namespace DelitaTrade.Core.ModelBinders
{
    public class InvoiceDetailModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            List<int> invoiceIds = new();
            var data = bindingContext.HttpContext.Request.Query;

            if (data.Count != 0)
            {
                data.TryGetValue("invoiceIds", out StringValues result);

                var stringId = result.FirstOrDefault() ?? string.Empty;

                foreach (var id in stringId.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    if (int.TryParse(id, out int intId))
                    {
                        invoiceIds.Add(intId);
                    }
                    else
                    {
                        bindingContext.Result = ModelBindingResult.Failed();
                    }
                }
            }

            if (invoiceIds.Count != 0 == false) 
            {
                bindingContext.Result = ModelBindingResult.Failed();
            }

            bindingContext.Result = ModelBindingResult.Success(invoiceIds);
            return Task.CompletedTask;
        }
    }
}
