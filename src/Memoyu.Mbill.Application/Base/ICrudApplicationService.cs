/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Application.Base
*   文件名称 ：ICrudApplicationService.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-02 11:10:02
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Memoyu.Mbill.Application.Contracts.Dtos.Base;
using Memoyu.Mbill.ToolKits.Base.Page;
using System.Threading.Tasks;

namespace Memoyu.Application.Base
{
    public interface ICrudApplicationService<TGetOutputDto, TGetListOutputDto, in TKey, in TGetListInput, in TCreateInput, in TUpdateInput>
        where TGetOutputDto : IEntityDto<TKey>
        where TGetListOutputDto : IEntityDto<TKey>
    {
        Task<PagedDto<TGetListOutputDto>> GetListAsync(TGetListInput input);

        Task<TGetOutputDto> GetAsync(TKey id);

        Task<TGetOutputDto> CreateAsync(TCreateInput createInput);

        Task<TGetOutputDto> UpdateAsync(TKey id, TUpdateInput updateInput);

        Task DeleteAsync(TKey id);
    }
}
