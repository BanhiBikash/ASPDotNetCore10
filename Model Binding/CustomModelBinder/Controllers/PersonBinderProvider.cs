using CustomModelBinder.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace CustomModelBinder.Controllers
{
    public class PersonBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(Models.Person))
            {
                return new BinderTypeModelBinder(typeof(PersonBinderProvider));
            }
            else
            {
                return null;
            }
        }
    }
}
