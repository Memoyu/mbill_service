using AutoMapper;
using mbill_service.Controllers.Core;
using mbill_service.Core.AOP.Attributes;
using mbill_service.Core.Domains.Common;
using mbill_service.Core.Domains.Common.Consts;
using mbill_service.Core.Domains.Entities.Bill;
using mbill_service.Service.Bill.Category;
using mbill_service.Service.Bill.Category.Input;
using mbill_service.Service.Bill.Category.Output;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mbill_service.Controllers.Bill
{
    /// <summary>
    /// 账单分类管理
    /// </summary>
    [Route("api/category")]
    public class CategoryController : ApiControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        /// <summary>
        /// 新增账单分类
        /// </summary>
        /// <param name="dto">账单分类</param>
        [Logger("用户新建了一个账单分类")]
        [HttpPost("create")]
        [Authorize]
        [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
        public async Task<ServiceResult> CreateAsync([FromBody] ModifyCategoryDto dto)
        {
            await _categoryService.InsertAsync(_mapper.Map<CategoryEntity>(dto));
            return ServiceResult.Successed("账单分类创建成功");
        }

        /// <summary>
        /// 获取分类
        /// </summary>
        /// <param name="id">分类id</param>
        [HttpGet("get")]
        [Authorize]
        [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
        public async Task<ServiceResult<CategoryDto>> GetAsync([FromQuery] long id)
        {
            return ServiceResult<CategoryDto>.Successed(await _categoryService.GetAsync(id));
        }

        /// <summary>
        /// 获取分类父项
        /// </summary>
        /// <param name="id">分类id</param>
        [HttpGet("parent/get")]
        [Authorize]
        [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
        public async Task<ServiceResult<CategoryDto>> GetParentAsync([FromQuery] long id)
        {
            return ServiceResult<CategoryDto>.Successed(await _categoryService.GetParentAsync(id));
        }

        /// <summary>
        /// 获取分组后的账单分类
        /// </summary>
        /// <param name="type">账单类型</param>
        [HttpGet("groups")]
        [Authorize]
        [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
        public async Task<ServiceResult<IEnumerable<CategoryGroupDto>>> GetGroupAsync([FromQuery] string type)
        {
            return ServiceResult<IEnumerable<CategoryGroupDto>>.Successed(await _categoryService.GetGroupsAsync(type));
        }
    }
}
