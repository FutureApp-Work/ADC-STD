using dotnet.Models.Testing;
using Microsoft.EntityFrameworkCore;

namespace dotnet.Services.Testing
{
  public class TestingDBContext(DbContextOptions<TestingDBContext> options) : DbContext(options)
  {
    #region Properties ####################################################################################################################

    public virtual DbSet<UserEntity> User { get; set; }


    #endregion ############################################################################################################################

    #region Public Functions ##############################################################################################################

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      // 將 Enum 存為字串
      foreach (var entityType in modelBuilder.Model.GetEntityTypes())
      {
        var properties = entityType.ClrType.GetProperties()
            .Where(p => p.PropertyType.IsEnum);

        foreach (var prop in properties)
        {
          modelBuilder.Entity(entityType.Name)
              .Property(prop.Name)
              .HasConversion<string>();
        }
      }
    }

    #endregion ############################################################################################################################

    #region Private Functions #############################################################################################################



    #endregion ############################################################################################################################
  }
}
