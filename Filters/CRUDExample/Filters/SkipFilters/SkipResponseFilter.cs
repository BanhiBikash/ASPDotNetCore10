using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.SkipFilters
{
    public class SkipResponseFilter:Attribute,IFilterMetadata
    {
    }
}
