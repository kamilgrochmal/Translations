using Microsoft.AspNetCore.Mvc.Rendering;
using Translations.Models.Admin;

namespace Translations.Helpers;

public static class SelectListHelper
{
    public static IEnumerable<SelectListItem> GenerateSelectList(List<ListItemDto> list)
    {
        List<SelectListItem> selectList = list
            .Select(n =>
                new SelectListItem
                {
                    Value = n.Value,
                    Text = n.Text
                }).ToList();
        return new SelectList(list, "Value", "Text");
    }
}