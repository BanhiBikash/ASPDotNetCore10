using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManager.UI.Filters.SkipFilters
{
    public class SkipResponseFilter:Attribute,IFilterMetadata
    {
    }
}
