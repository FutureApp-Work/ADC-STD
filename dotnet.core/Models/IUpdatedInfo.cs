namespace dotnet.Core
{
  public interface IUpdatedInfo
  {
    DateTimeOffset UpdatedAt { get; set; }

    ulong UpdaterId { get; set; }
  }
}
