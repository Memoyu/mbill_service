namespace Mbill.Controllers.Core;

[Route("api/dataseed")]
[ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v2)]
[Authorize]
public class DataSeedController : ApiControllerBase
{
    public DataSeedController()
    {
        
    }
}
