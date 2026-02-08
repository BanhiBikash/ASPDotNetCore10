using Microsoft.AspNetCore.Mvc.ModelBinding;
using CustomModelBinder.Models;

namespace CustomModelBinder.CustomModelBinders
{
    public class NameBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            Person person = new Person();
            person.PersonName = string.Empty;
            person.FullAddress = string.Empty;
            //person.Name = string.Empty;
            if (bindingContext.ValueProvider.GetValue("FirstName").Length > 0)
            {
                person.PersonName = bindingContext.ValueProvider.GetValue("FirstName").FirstValue;
            }

            if (bindingContext.ValueProvider.GetValue("LastName").Length > 0)
            {
                person.PersonName +=" " + bindingContext.ValueProvider.GetValue("LastName").FirstValue;
            }

            if (bindingContext.ValueProvider.GetValue("Address").Length > 0)
            {
                person.FullAddress = bindingContext.ValueProvider.GetValue("Address").FirstValue;
            }

            if (bindingContext.ValueProvider.GetValue("City").Length > 0)
            {
                person.FullAddress=String.Join(" ",person.FullAddress, bindingContext.ValueProvider.GetValue("City").FirstValue);  
            }

            if (bindingContext.ValueProvider.GetValue("Pin").Length > 0)
            {
                person.FullAddress=String.Join(" ",person.FullAddress,bindingContext.ValueProvider.GetValue("Pin").FirstValue.ToString());
            }

            bindingContext.Result = ModelBindingResult.Success(person);

            return Task.CompletedTask;
        }
    }
}
