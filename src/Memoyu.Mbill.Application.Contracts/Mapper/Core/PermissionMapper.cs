/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Application.Contracts.Mapper.Core
*   文件名称 ：PermissionMapper.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-14 22:48:14
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using AutoMapper;
using Memoyu.Mbill.Application.Contracts.Dtos.Core;
using Memoyu.Mbill.Domain.Entities.Core;

namespace Memoyu.Mbill.Application.Contracts.Mapper.Core
{
    public class PermissionMapper : Profile
    {
        public PermissionMapper()
        {
            CreateMap<PermissionEntity, PermissionDto>();
            CreateMap<ModifyPermissionDto, PermissionEntity>();
        }
    }
}
