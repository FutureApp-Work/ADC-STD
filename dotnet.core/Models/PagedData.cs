using X.PagedList.Extensions;

namespace dotnet.Core
{
  public class PagedData<TModel>
  {
    #region Properties ####################################################################################################################

    public PagedInfo? Paged { get; set; }

    public TModel? Data { get; set; }

    public IEnumerable<TModel>? List { get; set; }

    #endregion ############################################################################################################################

    #region Public Functions ##############################################################################################################

    public void PagedAndSort(IEnumerable<TModel> data)
    {
      var result = data.ToPagedList(Paged!.Index, Paged.Size);
      Paged.Count = result.TotalItemCount;
      List = result.AsEnumerable();
    }

    #endregion ############################################################################################################################

    #region Private Functions #############################################################################################################



    #endregion ############################################################################################################################
  }
}
