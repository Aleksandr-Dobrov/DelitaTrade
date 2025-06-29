using Microsoft.AspNetCore.Razor.TagHelpers;

namespace DelitaTrade.WebApp.TagHelpers
{
    [HtmlTargetElement("input")]
    public class DataItemIdTagHelper : TagHelper
    {
        public string? ItemId { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context.AllAttributes.TryGetAttribute("item-id", out var itemIdAttribute))
            {
                output.Attributes.SetAttribute("item-id", ItemId);
            }
        }
    }
}
