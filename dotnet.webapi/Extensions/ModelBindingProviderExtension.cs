using dotnet.Core;

namespace dotnet.Extensions
{
  public static class ModelBindingProviderExtension
  {
    #region Properties ####################################################################################################################



    #endregion ############################################################################################################################

    #region Public Functions ##############################################################################################################

    public static IServiceCollection AddModelBindingProvider(this IServiceCollection services)
    {
      services.AddControllers(options =>
      {
        options.ModelBinderProviders.Insert(0, new ModelBindingProvider());
      });

      return services;
    }

    #endregion ############################################################################################################################

    #region Private Functions #############################################################################################################



    #endregion ############################################################################################################################
  }
}
