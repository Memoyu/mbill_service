namespace mbill_service.Service.Base;

public abstract class CrudApplicationSvc<TEntity, TGetOutputDto, TGetListOutputDto, TKey, TGetListInput, TCreateInput,
            TUpdateInput>
        : ApplicationSvc, ICrudApplicationSvc<TGetOutputDto, TGetListOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
        where TEntity : class, IEntity<TKey>
        where TGetOutputDto : IEntityDto<TKey>
        where TGetListOutputDto : IEntityDto<TKey>
{
    /// <summary>
    /// 仓储
    /// </summary>
    protected IAuditBaseRepo<TEntity, TKey> Repository { get; }

    protected CrudApplicationSvc(IAuditBaseRepo<TEntity, TKey> repository)
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

    public virtual async Task<PagedDto<TGetListOutputDto>> GetListAsync(TGetListInput input)
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
            return query.Page(pageDto.Page, pageDto.Size);
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
