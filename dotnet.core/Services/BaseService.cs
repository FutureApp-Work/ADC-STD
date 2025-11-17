using Microsoft.EntityFrameworkCore;

namespace dotnet.Core
{
  public class BaseService<TEntity, TKey>(
    DbContext dbContext,
    ISessionContext session)
    : IDisposable
    where TEntity : Entity
    where TKey : IComparable<TKey>
  {
    #region Properties ####################################################################################################################

    private readonly DbContext _dbContext = dbContext;

    private readonly ISessionContext _session = session;
    private DbSet<TEntity> DbSet { get => _dbContext.Set<TEntity>(); }

    private IQueryable<TEntity> All { get => DbSet.AsQueryable(); }

    #endregion ############################################################################################################################

    #region Public Functions ##############################################################################################################

    public Task<bool> CreateAsync(TEntity entity)
    {
      CreateCore(entity);

      return CommitAsync();
    }

    public Task<bool> UpdateAsync(TEntity entity)
    {
      UpdateCore(entity);

      return CommitAsync();
    }

    public Task<bool> DeleteAsync(TEntity entity)
    {
      DeleteCore(entity);

      return CommitAsync();
    }

    public Task<TEntity?> GetByIdAsync(TKey id)
    {
      var qry = GetAvailable();

      if (typeof(IIdentity<TKey>).IsAssignableFrom(typeof(TEntity)))
      {
        var filter = (IQueryable<IIdentity<TKey>>)qry;

        qry = filter.Where(x => x.Id.Equals(id)).Cast<TEntity>();
      }

      return qry.FirstOrDefaultAsync();
    }

    public IQueryable<TEntity> GetAvailable()
    {
      var qry = All;

      if (typeof(ICodeState).IsAssignableFrom(typeof(TEntity)))
      {
        var filter = ((IQueryable<ICodeState>)qry);

        qry = filter.Where(x => x.State != StateEnum.D).Cast<TEntity>();
      }

      return qry;
    }

    public IQueryable<TEntity> GetAvailable(Func<TEntity, bool>? func = null)
    {
      var rtn = GetAvailable();

      if (func != null)
      {
        rtn = rtn.Where(func).AsQueryable();
      }

      return rtn;
    }

    public void Dispose()
    {
      _dbContext?.Dispose();
    }

    #endregion ############################################################################################################################

    #region Private Functions #############################################################################################################

    private void CreateCore(TEntity entity)
    {
      if (entity is ICodeState stat && stat.State == StateEnum.None)
      {
        stat.State = StateEnum.Y;
      }

      if (entity is ICodeSorting sort && sort.Sorting == 0)
      {
        sort.Sorting = 9999;
      }

      if (entity is ICreatedInfo created)
      {
        created.CreatedAt = DateTimeOffset.Now;
        created.CreaterId = created.CreaterId == 0 ? _session.CurrentUserId() : created.CreaterId;
      }

      if (entity is IUpdatedInfo updated)
      {
        updated.UpdatedAt = DateTimeOffset.Now;
        updated.UpdaterId = updated.UpdaterId == 0 ? _session.CurrentUserId() : updated.UpdaterId;
      }

      DbSet.Add(entity);
    }

    private void UpdateCore(TEntity entity)
    {
      if (entity is IUpdatedInfo updated)
      {
        updated.UpdatedAt = DateTimeOffset.Now;
        updated.UpdaterId = updated.UpdaterId == 0 ? _session.CurrentUserId() : updated.UpdaterId;
      }
    }

    private void DeleteCore(TEntity entity)
    {
      if (entity is ICodeState stat)
      {
        stat.State = StateEnum.D;

        UpdateCore(entity);
      }
      else
      {
        DbSet.Remove(entity);
      }
    }

    private async Task<bool> CommitAsync()
    {
      return !(await _dbContext.SaveChangesAsync()).Equals(0);
    }

    #endregion ############################################################################################################################
  }
}
