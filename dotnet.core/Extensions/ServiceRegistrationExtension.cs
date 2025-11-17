using Microsoft.Extensions.DependencyInjection;

namespace dotnet.Core
{
  public static class ServiceRegistrationExtension
  {
    #region Properties ####################################################################################################################

    private static readonly Type[] _exceptTypes = [
      typeof(IOperationInjection),
      typeof(IOperationSingleton),
      typeof(IOperationScoped),
      typeof(IOperationTransient),
      typeof(IDisposable),
    ];

    #endregion ############################################################################################################################

    #region Public Functions ##############################################################################################################

    public static IServiceCollection AddServiceRegistration(this IServiceCollection services)
    {
      foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
      {
        foreach (var tp in assembly.GetTypes()
                                   .Where(x => x.IsClass &&
                                               !x.IsAbstract &&
                                               x.IsAssignableTo(typeof(IOperationInjection))))
        {
          var face = tp.GetInterfaces()
                      .Except(_exceptTypes)
                      .Where(x => !string.IsNullOrEmpty(x.FullName) && !x.FullName.ToUpper().StartsWith("MICROSOFT"))
                      .FirstOrDefault();

          if (tp.IsAssignableTo(typeof(IOperationSingleton)))
          {
            services.AddSingleton(face ?? tp, tp);
          }
          else if (tp.IsAssignableTo(typeof(IOperationScoped)))
          {
            services.AddScoped(face ?? tp, tp);
          }
          else if (tp.IsAssignableTo(typeof(IOperationTransient)))
          {
            services.AddTransient(face ?? tp, tp);
          }
        }
      }

      return services;
    }

    #endregion ############################################################################################################################

    #region Private Functions #############################################################################################################



    #endregion ############################################################################################################################
  }

  public interface IOperationInjection { }

  public interface IOperationSingleton : IOperationInjection { }

  public interface IOperationScoped : IOperationInjection { }

  public interface IOperationTransient : IOperationInjection { }
}
