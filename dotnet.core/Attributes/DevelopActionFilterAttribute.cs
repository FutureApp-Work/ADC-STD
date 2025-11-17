using Microsoft.AspNetCore.Mvc.Filters;

namespace dotnet.Core
{
  public class DevelopActionFilterAttribute(IConfigurationService config) : ActionFilterAttribute, IOperationScoped
  {
    #region Properties ####################################################################################################################

    private readonly IConfigurationService _config = config;

    #endregion ############################################################################################################################

    #region Public Functions ##############################################################################################################

    public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
      NotImplementedException.ThrowIfNotDevelopment(_config.IsDevelopment);

      return base.OnActionExecutionAsync(context, next);
    }

    #endregion ############################################################################################################################

    #region Private Functions #############################################################################################################



    #endregion ############################################################################################################################

  }
}
