using Memo.Bill.Application.Aggregations.Queries;

namespace Memo.Bill.Api.Controllers
{
    /// <summary>
    /// 聚合接口
    /// </summary>
    public class AggregationController(ISender mediator) : ApiControllerBase
    {
        /// <summary>
        /// 获取经纬度对应地理位置信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("address/get")]
        public async Task<Result> GetAddressnAsync([FromQuery] GetAddressQuery request)
        {
            return await mediator.Send(request);
        }

        /// <summary>
        /// 获取天气预报信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("weatherinfo/get")]
        public async Task<Result> GetWeatherInfoAsync([FromQuery] GetWeatherInfoQuery request)
        {
            return await mediator.Send(request);
        }
    }
}
