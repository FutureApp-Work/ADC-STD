namespace dotnet.Core
{
  public sealed class DatabaseException : BaseException
  {
    private DatabaseException() : base(ExceptionEnum.DatabaseAccessFailed) { }

    private DatabaseException(ExceptionEnum exp) : base(exp) { }

    #region Properties ####################################################################################################################



    #endregion ############################################################################################################################

    #region Public Functions ##############################################################################################################

    public static void ThrowIfAccessFailed(Func<bool> isFailed)
    {
      if (isFailed())
      {
        throw new DatabaseException();
      }
    }

    public static void ThrowIfEmptyResult(Func<bool> isEmpty)
    {
      if (isEmpty())
      {
        throw new DatabaseException(ExceptionEnum.DatabaseEmptyResult);
      }
    }

    #endregion ############################################################################################################################

    #region Private Functions #############################################################################################################



    #endregion ############################################################################################################################
  }
}
