using dotnet.Core;

namespace dotnet.Models
{
  public class SessionContext: ISessionContext, IOperationScoped
  {
    // TODO: read user id
    public ulong CurrentUserId() => 9;
  }
}
