namespace dotnet.Extensions
{
  public static class SupportedCultureExtension
  {
    #region Properties ####################################################################################################################



    #endregion ############################################################################################################################

    #region Public Functions ##############################################################################################################

    public static IServiceCollection AddSupportedCulture(this IServiceCollection services, IConfiguration config)
    {
      var cultures = config.GetSection(("SupportedCultures"))
                           .Get<string[]>() ?? ["en-US"];

      services.Configure<RequestLocalizationOptions>(options =>
               {
                 options.SetDefaultCulture(cultures[0])
                        .AddSupportedCultures(cultures)
                        .AddSupportedUICultures(cultures);
               });

      return services;
    }

    #endregion ############################################################################################################################

    #region Private Functions #############################################################################################################



    #endregion ############################################################################################################################
  }
}
