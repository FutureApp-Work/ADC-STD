namespace dotnet.Core
{
  public class ResponseViewModel<TModel> : PagedData<TModel>
  {
    private ResponseViewModel() { }

    public ResponseViewModel(BaseException exp)
    {
      Code = exp.Code;
      Message = exp.Message;
    }

    public ResponseViewModel(PagedInfo? paged, IEnumerable<TModel>? model)
    {
      List = model;
      Paged = paged;
    }

    public ResponseViewModel(TModel model)
    {
      Data = model;
    }

    #region Properties ####################################################################################################################

    public int Code { get; set; } = 0;

    public string Message { get; set; } = string.Empty;

    public long Timestamp { get => DateTimeOffset.Now.ToUnixTimeMilliseconds(); }

    #endregion ############################################################################################################################

    #region Public Functions ##############################################################################################################



    #endregion ############################################################################################################################

    #region Private Functions #############################################################################################################



    #endregion ############################################################################################################################
  }
}
