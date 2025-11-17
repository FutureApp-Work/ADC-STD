using Asp.Versioning;

namespace dotnet.Extensions
{
  public static class APIVersioningExtension
  {
    #region Properties ####################################################################################################################



    #endregion ############################################################################################################################

    #region Public Functions ##############################################################################################################

    public static IServiceCollection AddAPIVersioning(this IServiceCollection services)
    {
      services.AddApiVersioning(options =>
               {
                 options.DefaultApiVersion = new ApiVersion(1, 0);
                 options.AssumeDefaultVersionWhenUnspecified = true;
                 options.ApiVersionReader = ApiVersionReader.Combine([
                   new HeaderApiVersionReader(["x-api-version", "api-version"]),
                   new UrlSegmentApiVersionReader(),
                 ]);
               })
               .AddApiExplorer(options =>
               {
                 options.GroupNameFormat = "'v'VVV";
                 options.SubstituteApiVersionInUrl = true;
               });

      return services;
    }

    #endregion ############################################################################################################################

    #region Private Functions #############################################################################################################



    #endregion ############################################################################################################################
  }
}
