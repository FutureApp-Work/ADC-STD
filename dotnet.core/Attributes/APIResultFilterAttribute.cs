using Microsoft.AspNetCore.Mvc;
using Filter = Microsoft.AspNetCore.Mvc.Filters;

namespace dotnet.Core
{
  public class APIResultFilterAttribute() : Filter::ResultFilterAttribute
  {
    #region Properties ####################################################################################################################

    #endregion ############################################################################################################################

    #region Public Functions ##############################################################################################################

    public override Task OnResultExecutionAsync(Filter::ResultExecutingContext context, Filter::ResultExecutionDelegate next)
    {
      if (context.Result is ObjectResult result)
      {
        context.Result = new ObjectResult(new APIResponseViewModel(result.Value));
      }

      return base.OnResultExecutionAsync(context, next);
    }

    #endregion ############################################################################################################################

    #region Private Functions #############################################################################################################



    #endregion ############################################################################################################################
  }
}
