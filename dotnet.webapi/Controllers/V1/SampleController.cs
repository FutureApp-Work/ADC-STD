using Asp.Versioning;
using dotnet.Core;
using dotnet.Models;
using dotnet.Services.Testing;
using Microsoft.AspNetCore.Mvc;
using Core = dotnet.Core;

namespace dotnet.Controllers.V1
{
  [ApiVersion("1.0")]
  [ServiceFilter<DevelopActionFilterAttribute>]
  public class SampleController(UserService user)
    : APIBaseController<UserViewmodel, UserViewmodel, ulong>
  {
    #region Properties ####################################################################################################################

    protected readonly UserService _user = user;

    #endregion ############################################################################################################################

    #region Public Functions ##############################################################################################################

    public override async Task<UserViewmodel> CreateAsync([FromBody] UserViewmodel model)
    {
      Core::ModelValidationException.ThrowIfInvalid(ModelState);

      model.JoinAt = DateTimeOffset.Now;

      var result = await _user.CreateAsync(model);
      Core::DatabaseException.ThrowIfAccessFailed(() => !result);

      return model;
    }

    public override async Task<UserViewmodel> UpdateAsync([FromBody] UserViewmodel model)
    {
      if (model.Id == 0)
      {
        ModelState.AddModelError("Id", "must > 0");
      }

      Core::ModelValidationException.ThrowIfInvalid(ModelState);

      var entity = await _user.GetByIdAsync(model.Id);
      Core::DatabaseException.ThrowIfEmptyResult(() => entity == null || entity.Id != model.Id);

      entity!.Name = model.Name;
      entity.Gender = model.Gender;
      entity.Birthday = model.Birthday;
      entity.Sorting = model.Sorting;

      var result = await _user.UpdateAsync(entity);
      Core::DatabaseException.ThrowIfAccessFailed(() => !result);

      model.Id = entity.Id;
      model.UpdatedAt = entity.UpdatedAt;
      model.UpdaterId = entity.UpdaterId;

      return model;
    }

    public override async Task<UserViewmodel?> GetAsync([FromRoute] ulong id)
    {
      if (id == 0)
      {
        ModelState.AddModelError("Id", "must > 0");
      }

      Core::ModelValidationException.ThrowIfInvalid(ModelState);

      var entity = await _user.GetByIdAsync(id);
      Core::DatabaseException.ThrowIfEmptyResult(() => entity == null || entity.Id != id);

      return new UserViewmodel
      {
        Id = entity!.Id,
        Name = entity.Name,
        Birthday = entity.Birthday,
        JoinAt = entity.JoinAt,
        Gender = entity.Gender,
        State = entity.State,
      };
    }

    public override async Task<UserViewmodel> DeleteAsync([FromRoute] ulong id)
    {
      if (id == 0)
      {
        ModelState.AddModelError("Id", "must > 0");
      }

      Core::ModelValidationException.ThrowIfInvalid(ModelState);

      var entity = await _user.GetByIdAsync(id);
      Core::DatabaseException.ThrowIfEmptyResult(() => entity == null || entity.Id != id);

      var result = await _user.DeleteAsync(entity!);
      Core::DatabaseException.ThrowIfAccessFailed(() => !result);

      return new UserViewmodel { Id = id };
    }

    public override Task<PagedData<UserViewmodel>> ListAsync([FromBody] PagedData<UserViewmodel> model)
    {
      var data = _user.GetAvailable()
                      .OrderBy(x => x.Sorting)
                      .ThenByDescending(x => x.Id)
                      .Select(x => new UserViewmodel
                      {
                        Id = x.Id,
                        Name = x.Name,
                        Birthday = x.Birthday,
                        JoinAt = x.JoinAt,
                        Gender = x.Gender,
                        State = x.State,
                      });
      model.PagedAndSort(data);

      return Task.FromResult(model);
    }

    #endregion ############################################################################################################################

    #region Private Functions #############################################################################################################



    #endregion ############################################################################################################################
  }
}
