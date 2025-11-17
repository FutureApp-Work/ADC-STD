using System.Text.Json.Serialization;

namespace dotnet.Extensions
{
  public static class JsonSerializerOptionExtension
  {
    #region Properties ####################################################################################################################



    #endregion ############################################################################################################################

    #region Public Functions ##############################################################################################################

    public static IServiceCollection AddJsonSerializerOption(this IServiceCollection services)
    {
      services.AddControllers()
              .AddJsonOptions(options =>
              {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
              });

      return services;
    }

    #endregion ############################################################################################################################

    #region Private Functions #############################################################################################################



    #endregion ############################################################################################################################
  }
}
