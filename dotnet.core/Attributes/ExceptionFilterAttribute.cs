using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Filter = Microsoft.AspNetCore.Mvc.Filters;

namespace dotnet.Core
{
  public class ExceptionFilterAttribute : Filter::ExceptionFilterAttribute
  {
    #region Properties ####################################################################################################################



    #endregion ############################################################################################################################

    #region Public Functions ##############################################################################################################

    public override Task OnExceptionAsync(ExceptionContext context)
    {
      if (!context.ExceptionHandled)
      {
        if (context.Exception is not BaseException exp)
        {
          exp = ServerErrorException.Internal();
        }

        context.Result = new ObjectResult(new APIResponseViewModel(exp)) { StatusCode = exp.StatusCode };
        context.ExceptionHandled = true;
      }


      return base.OnExceptionAsync(context);
    }

    #endregion ############################################################################################################################

    #region Private Functions #############################################################################################################



    #endregion ############################################################################################################################
  }
}
