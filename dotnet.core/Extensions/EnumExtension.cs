using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Reflection;

namespace dotnet.Core
{
  public static class EnumExtension
  {
    #region Properties ####################################################################################################################



    #endregion ############################################################################################################################

    #region Public Functions ##############################################################################################################

    public static string GetDisplayName(this Enum val)
    {
      return GetAttr<DisplayAttribute>(val)?.GetName() ?? val.ToString();
    }

    public static int GetHttpStatusCode(this Enum val, HttpStatusCode code = HttpStatusCode.OK)
    {
      return GetAttr<HttpStatusCodeAttribute>(val)?.GetCode() ?? (int)code;
    }

    #endregion ############################################################################################################################

    #region Private Functions #############################################################################################################

    private static TA? GetAttr<TA>(Enum val)
      where TA : Attribute
    {
      var field = val.GetType().GetField(val.ToString());
      if (field == null)
      {
        return null;
      }

      return field.GetCustomAttribute<TA>();
    }

    #endregion ############################################################################################################################
  }
}
