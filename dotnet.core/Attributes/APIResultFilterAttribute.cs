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
      if (context.Result is ObjectResult result && result.Value != null)
      {
        // For APIController behavior - Model validation failed
        if (context.Result is BadRequestObjectResult bad)
        {
          try
          {
            ModelValidationException.ThrowIfInvalid(false);
          }
          catch (BaseException exp)
          {
            context.Result = new ObjectResult(new ResponseViewModel<object>(exp)) { StatusCode = exp.StatusCode };
          }
        }
        else if (result.Value.GetType().IsGenericType &&
                   result.Value.GetType().GetGenericTypeDefinition().IsAssignableTo(typeof(PagedData<>)))
        {
          var paged = (IPagedData<object>)result.Value;
          context.Result = new ObjectResult(new ResponseViewModel<object>(paged.GetPaged(), paged.GetList()));
        }
        else
        {
          context.Result = new ObjectResult(new ResponseViewModel<object>(result.Value));
        }
      }

      return base.OnResultExecutionAsync(context, next);
    }

    #endregion ############################################################################################################################

    #region Private Functions #############################################################################################################



    #endregion ############################################################################################################################
  }
}
