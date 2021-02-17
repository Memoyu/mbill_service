using AutoMapper;
using Memoyu.Mbill.Application.Bill.Asset;
using Memoyu.Mbill.Application.Contracts.Attributes;
using Memoyu.Mbill.Application.Contracts.Dtos.Bill.Asset;
using Memoyu.Mbill.Domain.Entities.Bill.Asset;
using Memoyu.Mbill.Domain.Shared.Const;
using Memoyu.Mbill.ToolKits.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Memoyu.Mbill.WebApi.Controllers.Bill
{
    /// <summary>
    /// 资产分类管理
    /// </summary>
    [Route("api/asset")]
    public class AssetController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAssetService _assetService;

        public AssetController(IAssetService assetService, IMapper mapper)
        {
            _mapper = mapper;
            _assetService = assetService;
        }

        /// <summary>
        /// 新增资产分类
        /// </summary>
        /// <param name="dto">资产分类</param>
        [Logger("用户新建了一个资产分类")]
        [HttpPost("create")]
        [Authorize]
        [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
        public async Task<ServiceResult> CreateAsync([FromBody] ModifyAssetDto dto)
        {
            await _assetService.InsertAsync(_mapper.Map<AssetEntity>(dto));
            return ServiceResult.Successed("资产分类创建成功");
        }

        /// <summary>
        /// 获取资产
        /// </summary>
        /// <param name="id">资产id</param>
        [HttpGet("get")]
        [Authorize]
        [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
        public async Task<ServiceResult<AssetDto>> GetAsync([FromQuery] long id)
        {
            return ServiceResult<AssetDto>.Successed(await _assetService.GetAsync(id));
        }

        /// <summary>
        /// 获取资产父项
        /// </summary>
        /// <param name="id">资产id</param>
        [HttpGet("parent/get")]
        [Authorize]
        [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
        public async Task<ServiceResult<AssetDto>> GetParentAsync([FromQuery] long id)
        {
            return ServiceResult<AssetDto>.Successed(await _assetService.GetParentAsync(id));
        }

        /// <summary>
        /// 获取分组后的资产
        /// </summary>
        /// <param name="type">资产类型</param>
        [HttpGet("groups")]
        [Authorize]
        [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
        public async Task<ServiceResult<IEnumerable<AssetGroupDto>>> GetGroupAsync([FromQuery] string type)
        {
            return ServiceResult<IEnumerable<AssetGroupDto>>.Successed(await _assetService.GetGroupsAsync(type));
        }
    }
}
