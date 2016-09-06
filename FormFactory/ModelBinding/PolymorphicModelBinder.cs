using System;
using System.Reflection;

namespace FormFactory.ModelBinding
{
    public class PolymorphicModelBinder
    {
        public PolymorphicModelBinder( )
        {
        }

        public object CreateModel(IModelBindingContext bindingContext, Type modelType, Func<object> fallback, Func<string, Type> readTypeFromString)
        {
            var typeSlug = bindingContext.ValueProvider.GetValue(bindingContext.ModelName + ".__type");
            if (typeSlug != null)
            {
                var value = typeSlug.AttemptedValue as string;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    var type = readTypeFromString(value);
                    if (type != null && modelType.GetTypeInfo().IsAssignableFrom(type.GetTypeInfo()))
                    {
                        var instance = fallback();
                        bindingContext.SetModelMetadataForType(() => instance, type);
                        return instance;
                    }
                }
            }
            return fallback();

        }

        public object BindModel(IModelBindingContext bindingContext, Func<Type, IModelBindingContext, object> bindToType, Func<object> defaultBind, Func<string, Type> readTypeFromString)
        {
            var typeSlug = bindingContext.ValueProvider.GetValue(bindingContext.ModelName + ".__type");
            if (typeSlug != null)
            {
                var value = typeSlug.AttemptedValue as string;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    var type = readTypeFromString(value);
                   
                    return bindToType(type, bindingContext);
                
                }
            }
            return defaultBind();
        }


        //public ICustomTypeDescriptor GetTypeDescriptor(IModelBindingContext bindingContext, Func<ICustomTypeDescriptor> fallback)
        //{
        //    if (bindingContext.Model != null)
        //    {
        //        var concreteType = bindingContext.Model.GetType();

        //        if (Nullable.GetUnderlyingType(concreteType) == null)
        //        {
        //            return new AssociatedMetadataTypeTypeDescriptionProvider(concreteType).GetTypeDescriptor(concreteType);
        //        }
        //    }

        //    return fallback();
        //}
    }
}