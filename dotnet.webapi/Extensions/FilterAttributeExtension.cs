using dotnet.Core;

namespace dotnet.Extensions
{
  public static class FilterAttributeExtension
  {
    #region Properties ####################################################################################################################



    #endregion ############################################################################################################################

    #region Public Functions ##############################################################################################################

    public static IServiceCollection AddFilterAttribute(this IServiceCollection services)
    {
      services.AddControllers(options =>
      {
        options.Filters.Add<APIResultFilterAttribute>();
        options.Filters.Add<ExceptionFilterAttribute>();
      });

      return services;
    }

    #endregion ############################################################################################################################

    #region Private Functions #############################################################################################################



    #endregion ############################################################################################################################
  }
}
