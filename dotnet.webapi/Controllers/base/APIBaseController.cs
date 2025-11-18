using Microsoft.AspNetCore.Mvc;
using Core = dotnet.Core;

namespace dotnet.Controllers
{
#pragma warning disable CS1591
  [ApiController]
  [Route("api/[controller]")]
  public abstract class APIBaseController<QModel, VModel, TKey> : ControllerBase
    where QModel : class, Core::IIdentity<TKey>, new()
    where VModel : class, Core::IIdentity<TKey>, new()
  {
    #region Properties ####################################################################################################################



    #endregion ############################################################################################################################

    #region Public Functions ##############################################################################################################

    [HttpGet("{id}")]
    public virtual Task<VModel> GetAsync([FromRoute] TKey id)
    {
      throw Core::NotImplementedException.Create();
    }

    [HttpPost("list")]
    public virtual Task<Core::PagedData<VModel>> ListAsync([FromBody] Core::PagedData<QModel> model)
    {
      throw Core::NotImplementedException.Create();
    }

    [HttpPost]
    public virtual Task<VModel> CreateAsync([FromBody] QModel model)
    {
      throw Core::NotImplementedException.Create();
    }

    [HttpPut]
    public virtual Task<VModel> UpdateAsync([FromBody] QModel model)
    {
      throw Core::NotImplementedException.Create();
    }

    [HttpDelete("{id}")]
    public virtual Task<VModel> DeleteAsync([FromRoute] TKey id)
    {
      throw Core::NotImplementedException.Create();
    }

    #endregion ############################################################################################################################

    #region Private Functions #############################################################################################################



    #endregion ############################################################################################################################
  }
#pragma warning disable CS1591
}
