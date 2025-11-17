namespace dotnet.Core
{
  public interface ICreatedInfo
  {
    DateTimeOffset CreatedAt { get; set; }

    ulong CreaterId { get; set; }
  }
}
