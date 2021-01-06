/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Application.Base.Impl
*   文件名称 ：CrudApplicationService.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-02 11:10:20
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using FreeSql;
using Memoyu.Application.Base;
using Memoyu.Mbill.Domain.Base;
using Memoyu.Mbill.ToolKits.Base.Page;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Memoyu.Mbill.Application.Base.Impl
{
    public abstract class CrudApplicationService<TEntity, TGetOutputDto, TGetListOutputDto, TKey, TGetListInput, TCreateInput,
            TUpdateInput>
        : ApplicationService, ICrudApplicationService<TGetOutputDto, TGetListOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
        where TEntity : class, IEntity<TKey>
        where TGetOutputDto : IEntityDto<TKey>
        where TGetListOutputDto : IEntityDto<TKey>
    {
        /// <summary>
        /// 仓储
        /// </summary>
        protected IAuditBaseRepository<TEntity, TKey> Repository { get; }

        protected CrudApplicationService(IAuditBaseRepository<TEntity, TKey> repository)
        {
            Repository = repository;
        }


        public async virtual Task<TGetOutputDto> CreateAsync(TCreateInput createInput)
        {
            TEntity entity = Mapper.Map<TEntity>(createInput);
            await Repository.InsertAsync(entity);
            return Mapper.Map<TGetOutputDto>(entity);
        }

        public async virtual Task DeleteAsync(TKey id)
        {
            await Repository.DeleteAsync(id);
        }

        public virtual async Task<TGetOutputDto> GetAsync(TKey id)
        {
            TEntity entity = await Repository.GetAsync(id);
            return Mapper.Map<TGetOutputDto>(entity);
        }

        public virtual async  Task<PagedDto<TGetListOutputDto>> GetListAsync(TGetListInput input)
        {
            var select = QueryAll();
            long totalCount = await select.CountAsync();
            ApplySorting(select, input);
            List<TEntity> entities = await ApplyPaging(select, input).ToListAsync();
            return new PagedDto<TGetListOutputDto>(entities.Select(MapToGetListOutputDto).ToList(), totalCount);
        }

        public virtual async Task<TGetOutputDto> UpdateAsync(TKey id, TUpdateInput updateInput)
        {
            TEntity entity = await GetEntityByIdAsync(id);
            Mapper.Map(updateInput, entity);
            await Repository.UpdateAsync(entity);
            return Mapper.Map<TGetOutputDto>(entity);
        }

        protected virtual ISelect<TEntity> QueryAll()
        {
            return Repository.Select;
        }
        protected virtual async Task<TEntity> GetEntityByIdAsync(TKey id)
        {
            return await Repository.GetAsync(id);
        }

        protected virtual ISelect<TEntity> ApplyPaging(ISelect<TEntity> query, TGetListInput input)
        {
            if (input is IPagingDto pageDto)
            {
                return query.Page(pageDto.Page + 1, pageDto.Size);
            }
            return query;
        }

        protected virtual ISelect<TEntity> ApplySorting(ISelect<TEntity> query, TGetListInput input)
        {
            if (input is ISortedResultRequest sortInput)
            {
                if (!string.IsNullOrWhiteSpace(sortInput.Sorting))
                {
                    return query.OrderBy(sortInput.Sorting);
                }
            }
            if (input is ILimitedResultRequest)
            {
                return query.OrderByDescending(e => e.Id);
            }
            return query;
        }
        protected virtual TGetListOutputDto MapToGetListOutputDto(TEntity entity)
        {
            return Mapper.Map<TEntity, TGetListOutputDto>(entity);
        }
    }
}
