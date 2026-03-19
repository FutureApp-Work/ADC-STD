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
            context.Result = new ObjectResult(new ResponseViewModel<object>(exp)) { StatusCode = 400 };
          }
        }
        // Already wrapped by the controller - don't double-wrap
        else if (result.Value.GetType().IsGenericType &&
                   result.Value.GetType().GetGenericTypeDefinition() == typeof(ResponseViewModel<>))
        {
          // No action - preserve the result as-is (including original status code)
        }
        else if (result.Value.GetType().IsGenericType &&
                   result.Value.GetType().GetGenericTypeDefinition().IsAssignableTo(typeof(PagedData<>)))
        {
          var paged = (IPagedData<object>)result.Value;
          context.Result = new ObjectResult(new ResponseViewModel<object>(paged.GetPaged(), paged.GetList()))
          {
            StatusCode = result.StatusCode
          };
        }
        else
        {
          context.Result = new ObjectResult(new ResponseViewModel<object>(result.Value))
          {
            StatusCode = result.StatusCode
          };
        }
      }

      return base.OnResultExecutionAsync(context, next);
    }

    #endregion ############################################################################################################################

    #region Private Functions #############################################################################################################



    #endregion ############################################################################################################################
  }
}
