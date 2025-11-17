namespace dotnet.Core
{
  public class APIResponseViewModel : PagedDataViewModel<object>
  {
    public APIResponseViewModel(BaseException exp)
    {
      Code = exp.Code;
      Message = exp.Message;
    }

    public APIResponseViewModel(object model)
    {
      if (model is IPagedDataViewModel paged)
      {
        List = paged.GetList();
        Paged = paged.GetPaged();
      }
      else
      {
        Data = model;
      }
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
