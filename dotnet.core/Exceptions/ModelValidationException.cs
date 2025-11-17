using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace dotnet.Core
{
  public sealed class ModelValidationException : BaseException
  {
    private ModelValidationException() : base(ExceptionEnum.ModelInvalid) { }

    #region Properties ####################################################################################################################



    #endregion ############################################################################################################################

    #region Public Functions ##############################################################################################################

    public static void ThrowIfInvalid(ModelStateDictionary state)
    {
      if (!state.IsValid)
      {
        throw new ModelValidationException();
      }
    }

    #endregion ############################################################################################################################

    #region Private Functions #############################################################################################################



    #endregion ############################################################################################################################
  }
}
