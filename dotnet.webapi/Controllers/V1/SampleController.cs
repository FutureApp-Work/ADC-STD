using Asp.Versioning;
using dotnet.Core;
using dotnet.Models.Testing;
using dotnet.Services.Testing;
using Microsoft.AspNetCore.Mvc;
using Core = dotnet.Core;

namespace dotnet.Controllers.V1
{
  [ApiVersion("1.0")]
  [ServiceFilter<DevelopActionFilterAttribute>]
  public class SampleController(UserService user)
    : APIBaseController<UserViewModel, UserViewModel, ulong>
  {
    #region Properties ####################################################################################################################

    protected readonly UserService _user = user;

    #endregion ############################################################################################################################

    #region Public Functions ##############################################################################################################

    public override async Task<UserViewModel> CreateAsync([FromBody] UserViewModel model)
    {
      Core::ModelValidationException.ThrowIfInvalid(ModelState);

      model.JoinAt = DateTimeOffset.Now;

      var result = await _user.CreateUserAsync(model);
      Core::DatabaseException.ThrowIfAccessFailed(() => !result);

      return model;
    }

    public override async Task<UserViewModel> UpdateAsync([FromBody] UserViewModel model)
    {
      if (model.Id == 0)
      {
        ModelState.AddModelError("Id", "must > 0");
      }
      Core::ModelValidationException.ThrowIfInvalid(ModelState);

      var result = await _user.UpdateUserAsync(model);

      if (!result)
      {
        Core::DatabaseException.ThrowIfEmptyResult(() => model.Id == 0);
      }
      Core::DatabaseException.ThrowIfAccessFailed(() => !result);

      return model;
    }

    public override async Task<UserViewModel> GetAsync([FromRoute] ulong id)
    {
      if (id == 0)
      {
        ModelState.AddModelError("Id", "must > 0");
      }
      Core::ModelValidationException.ThrowIfInvalid(ModelState);

      var model = await _user.GetUserByIdAsync(id);
      Core::DatabaseException.ThrowIfEmptyResult(() => model == null || model.Id != id);

      return model!;
    }

    public override async Task<UserViewModel> DeleteAsync([FromRoute] ulong id)
    {
      if (id == 0)
      {
        ModelState.AddModelError("Id", "must > 0");
      }
      Core::ModelValidationException.ThrowIfInvalid(ModelState);

      var result = await _user.DeleteUserByIdAsync(id);
      Core::DatabaseException.ThrowIfAccessFailed(() => !result);

      return new UserViewModel { Id = id };
    }

    public override Task<PagedData<UserViewModel>> ListAsync([FromBody] PagedData<UserViewModel> model)
    {
      var result = _user.PagedListAsync(model);
      Core::DatabaseException.ThrowIfEmptyResult(() => result.List == null);

      return Task.FromResult(result);
    }

    #endregion ############################################################################################################################

    #region Private Functions #############################################################################################################



    #endregion ############################################################################################################################
  }
}
