using dotnet.Core;
using dotnet.Models.Testing;

namespace dotnet.Services.Testing
{
  public class UserService(TestingDBContext dbContext, ISessionContext session)
    : BaseService<UserEntity, ulong>(dbContext, session), IOperationScoped
  {
    #region Properties ####################################################################################################################



    #endregion ############################################################################################################################

    #region Public Functions ##############################################################################################################

    public Task<bool> CreateUserAsync(UserViewModel model)
    {
      var entity = new UserEntity
      {
        Name = model.Name,
        Gender = model.Gender,
        JoinAt = model.JoinAt,
        Birthday = model.Birthday,
        State = model.State,
        Sorting = model.Sorting,
      };

      return CreateAsync(entity);
    }

    public async Task<bool> UpdateUserAsync(UserViewModel model)
    {
      var entity = await GetByIdAsync(model.Id);
      if (entity == null || entity.Id != model.Id)
      {
        return false;
      }

      entity!.Name = model.Name;
      entity.Gender = model.Gender;
      entity.Birthday = model.Birthday;
      entity.Sorting = model.Sorting;

      var result = await UpdateAsync(entity);
      if (result)
      {
        model.Id = entity.Id;
      }

      return result;
    }

    public async Task<UserViewModel?> GetUserByIdAsync(ulong id)
    {
      var entity = await GetByIdAsync(id);
      if (entity == null || entity.Id != id)
      {
        return null;
      }

      return new UserViewModel
      {
        Id = entity!.Id,
        Name = entity.Name,
        Gender = entity.Gender,
        JoinAt = entity.JoinAt,
        Birthday = entity.Birthday,
        State = entity.State,
        Sorting = entity.Sorting,
      };
    }

    public async Task<bool> DeleteUserByIdAsync(ulong id)
    {
      var entity = await GetByIdAsync(id);
      if (entity == null || entity.Id != id)
      {
        return false;
      }

      return await DeleteAsync(entity!);
    }

    public PagedData<UserViewModel> PagedListAsync(PagedData<UserViewModel> model)
    {
      var data = GetAvailable().OrderBy(x => x.Sorting)
                               .ThenByDescending(x => x.Id)
                               .Select(x => new UserViewModel
                               {
                                 Id = x.Id,
                                 Name = x.Name,
                                 Birthday = x.Birthday,
                                 JoinAt = x.JoinAt,
                                 Gender = x.Gender,
                                 State = x.State,
                                 Sorting = x.Sorting,
                               });
      model.PagedAndSort(data);

      return model;
    }

    #endregion ############################################################################################################################

    #region Private Functions #############################################################################################################



    #endregion ############################################################################################################################
  }
}
