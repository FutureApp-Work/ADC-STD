using System.ComponentModel.DataAnnotations;
using System.Net;

namespace dotnet.Core
{
  public class BaseException : Exception
  {
    private BaseException() { }

    protected BaseException(ExceptionEnum exp) : base(exp.GetDisplayName())
    {
      _exp = exp;
    }

    #region Properties ####################################################################################################################

    private readonly ExceptionEnum _exp;

    public int Code { get => (int)_exp; }

    public int StatusCode { get => _exp.GetHttpStatusCode(HttpStatusCode.InternalServerError); }

    #endregion ############################################################################################################################

    #region Public Functions ##############################################################################################################



    #endregion ############################################################################################################################

    #region Private Functions #############################################################################################################



    #endregion ############################################################################################################################
  }

  public enum ExceptionEnum
  {
    None = 0,

    [Display(Name = "ModelStateInvalid", ResourceType = typeof(ExceptionResource))]
    [HttpStatusCode(HttpStatusCode.UnprocessableContent)]
    ModelInvalid = 5600,

    [Display(Name = "DatabaseAccessFailed", ResourceType = typeof(ExceptionResource))]
    [HttpStatusCode(HttpStatusCode.InternalServerError)]
    DatabaseAccessFailed = 6200,

    [Display(Name = "DatabaseEmptyResult", ResourceType = typeof(ExceptionResource))]
    [HttpStatusCode(HttpStatusCode.InternalServerError)]
    DatabaseEmptyResult = 6260,

    [Display(Name = "NotImplemented", ResourceType = typeof(ExceptionResource))]
    [HttpStatusCode(HttpStatusCode.NotImplemented)]
    NotImplemented = 9899,

    [Display(Name = "SystemError", ResourceType = typeof(ExceptionResource))]
    [HttpStatusCode(HttpStatusCode.InternalServerError)]
    InternalServerError = 9999,
  }
}
