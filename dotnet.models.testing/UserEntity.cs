using System.ComponentModel.DataAnnotations.Schema;
using dotnet.Core;

namespace dotnet.Models.Testing
{
  [Table(name: "user")]
  public class UserEntity : Entity, IIdentity<ulong>, ICodeState, ICodeSorting, ICreatedInfo, IUpdatedInfo
  {
    #region Properties ####################################################################################################################

    [Column(name: "id_user")]
    public ulong Id { get; set; }

    [Column(name: "name_user")]
    public string Name { get; set; } = string.Empty;

    [Column(name: "gender")]
    public GenderEnum Gender { get; set; }

    [Column(name: "join_at_user")]
    public DateTimeOffset JoinAt { get; set; }

    [Column(name: "birthday")]
    public DateOnly Birthday { get; set; }

    [Column(name: "code_state")]
    public StateEnum State { get; set; }

    [Column(name: "code_sort")]
    public int Sorting { get; set; }

    [Column(name: "date_create")]
    public DateTimeOffset CreatedAt { get; set; }

    [Column(name: "id_create")]
    public ulong CreaterId { get; set; }

    [Column(name: "date_update")]
    public DateTimeOffset UpdatedAt { get; set; }

    [Column(name: "id_update")]
    public ulong UpdaterId { get; set; }

    #endregion ############################################################################################################################

    #region Public Functions ##############################################################################################################



    #endregion ############################################################################################################################

    #region Private Functions #############################################################################################################



    #endregion ############################################################################################################################
  }
}
