using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace dotnet.Core
{
  public sealed class ModelValidationException : BaseException
  {
    private ModelValidationException() : base(ExceptionEnum.ModelInvalid) { }

    #region Properties ####################################################################################################################



    #endregion ############################################################################################################################

    #region Public Functions ##############################################################################################################

    public static void ThrowIfInvalid(bool isValid)
    {
      if (!isValid)
      {
        throw new ModelValidationException();
      }
    }

    public static void ThrowIfInvalid(ModelStateDictionary state)
    {
      ThrowIfInvalid(state.IsValid);
    }

    #endregion ############################################################################################################################

    #region Private Functions #############################################################################################################



    #endregion ############################################################################################################################
  }
}
