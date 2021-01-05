/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Application.Contracts.Mapper.Test
*   文件名称 ：TestMapper.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 22:22:33
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using AutoMapper;
using Memoyu.Mbill.Application.Contracts.Test;
using Memoyu.Mbill.Domain.Entities;

namespace Memoyu.Mbill.Application.Contracts.Mapper.Test
{
    public class TestMapper : Profile
    {
        public TestMapper()
        {
            CreateMap<ModifyTestDto, TestEntity>();
        }
    }
}
