using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace dotnet.Core
{
  public class ModelBindingProvider : IModelBinderProvider
  {
    #region Properties ####################################################################################################################



    #endregion ############################################################################################################################

    #region Public Functions ##############################################################################################################

    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
      ArgumentNullException.ThrowIfNull(context);

      if (context.Metadata.IsEnum)
      {
        return new BinderTypeModelBinder(typeof(EnumModelBinder));
      }

#pragma warning disable CS8603 // 可能有 Null 參考傳回。
      return null;
#pragma warning restore CS8603 // 可能有 Null 參考傳回。
    }

    #endregion ############################################################################################################################

    #region Private Functions #############################################################################################################



    #endregion ############################################################################################################################
  }

  public class EnumModelBinder : IModelBinder
  {
    #region Properties ####################################################################################################################



    #endregion ############################################################################################################################

    #region Public Functions ##############################################################################################################

    public Task BindModelAsync(ModelBindingContext context)
    {
      ArgumentNullException.ThrowIfNull(context);

      if (!context.ModelType.IsEnum)
      {
        return Task.CompletedTask;
      }

      var valueResult = context.ValueProvider.GetValue(context.ModelName);
      if (valueResult == ValueProviderResult.None)
      {
        return Task.CompletedTask;
      }

      context.ModelState.SetModelValue(context.ModelName, valueResult);

      if (string.IsNullOrEmpty(valueResult.FirstValue))
      {
        return Task.CompletedTask;
      }

      try
      {
        var enumValue = Enum.Parse(context.ModelType, valueResult.FirstValue, true);
        context.Result = ModelBindingResult.Success(enumValue);
      }
      catch { }

      return Task.CompletedTask;
    }

    #endregion ############################################################################################################################

    #region Private Functions #############################################################################################################



    #endregion ############################################################################################################################
  }
}
