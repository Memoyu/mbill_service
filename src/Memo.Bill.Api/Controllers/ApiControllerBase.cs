namespace Memo.Bill.Api.Controllers;

/// <summary>
/// API基类
/// </summary>
[ApiController]
[Route("api/v2/[controller]")]
[Authorize]
public abstract class ApiControllerBase : ControllerBase
{
}
