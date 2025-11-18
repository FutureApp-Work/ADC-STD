using dotnet.Core;

namespace dotnet.Models.Testing
{
  public class UserViewModel : IIdentity<ulong>
  {
    #region Properties ####################################################################################################################

    public ulong Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public GenderEnum Gender { get; set; }

    public DateTimeOffset JoinAt { get; set; }

    public DateOnly Birthday { get; set; }

    public StateEnum State { get; set; }

    public int Sorting { get; set; }

    #endregion ############################################################################################################################

    #region Public Functions ##############################################################################################################



    #endregion ############################################################################################################################

    #region Private Functions #############################################################################################################



    #endregion ############################################################################################################################
  }
}
