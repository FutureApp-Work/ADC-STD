using System.Text.Json;
using System.Text.Json.Serialization;

namespace dotnet.Core
{
  public class JsonDatetimeOffsetProvider : JsonConverter<DateTimeOffset>
  {
    #region Properties ####################################################################################################################



    #endregion ############################################################################################################################

    #region Public Functions ##############################################################################################################

    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
      return reader.GetDateTimeOffset();
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
      writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:sszzz"));
    }

    #endregion ############################################################################################################################

    #region Private Functions #############################################################################################################



    #endregion ############################################################################################################################
  }
}
