using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PSL.MicroserviceTemplate.Domain.Primitives;

namespace PSL.MicroserviceTemplate.Api.ModelBinders
{
    public class PrimitiveModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            // Try to fetch the value of the argument by name
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (value == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            // Sets the value for the ModelStateEntry with the specified key
            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, value);

            // Create an instance of the Primitive ModelType
            var model = PrimitiveFactory.Create(bindingContext.ModelType, value.FirstValue);

            // Create a ModelBindingResult representing model binding operation outcome
            bindingContext.Result = ModelBindingResult.Success(model);

            return Task.CompletedTask;
        }
    }

    public class PrimitivesModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context) => context.Metadata.ModelType.IsAssignableTo(typeof(IPrimitive))
            ? new BinderTypeModelBinder(typeof(PrimitiveModelBinder)) : null;
    }
}
