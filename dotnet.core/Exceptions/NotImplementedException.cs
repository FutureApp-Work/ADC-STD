namespace dotnet.Core
{
  public sealed class NotImplementedException : BaseException
  {
    private NotImplementedException() : base(ExceptionEnum.NotImplemented) { }

    #region Properties ####################################################################################################################



    #endregion ############################################################################################################################

    #region Public Functions ##############################################################################################################

    public static NotImplementedException Create() => new();

    public static void ThrowIfNotDevelopment(bool isDev)
    {
      if (!isDev)
      {
        throw new NotImplementedException();
      }
    }

    #endregion ############################################################################################################################

    #region Private Functions #############################################################################################################



    #endregion ############################################################################################################################
  }
}
