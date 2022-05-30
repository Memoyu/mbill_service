namespace mbill.Service.Base;

public abstract class CrudApplicationSvc<TEntity, TGetOutputDto, TKey, TCreateInput, TUpdateInput>
        : ApplicationSvc, ICrudApplicationSvc<TGetOutputDto, TKey, TCreateInput, TUpdateInput>
        where TEntity : class, IEntity<TKey>
        where TGetOutputDto : IEntityDto<TKey>
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
}
