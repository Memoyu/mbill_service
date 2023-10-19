namespace mbill.Service.Base;

public abstract class CrudApplicationSvc<TEntity, TGetOutputDto, TSimpleOutputDto, TKey, TCreateInput, TUpdateInput>
        : ApplicationSvc, ICrudApplicationSvc<TGetOutputDto, TSimpleOutputDto, TKey, TCreateInput, TUpdateInput>
        where TEntity : class, IEntity
        where TGetOutputDto : IEntityDto
{
    /// <summary>
    /// 仓储
    /// </summary>
    protected IAuditBaseRepo<TEntity, TKey> Repository { get; }

    protected CrudApplicationSvc(IAuditBaseRepo<TEntity, TKey> repository)
    {
        Repository = repository;
    }


    public async virtual Task<ServiceResult<TSimpleOutputDto>> CreateAsync(TCreateInput createInput)
    {
        TEntity entity = Mapper.Map<TEntity>(createInput);
        await Repository.InsertAsync(entity);
        return ServiceResult<TSimpleOutputDto>.Successed(Mapper.Map<TSimpleOutputDto>(entity));
    }

    public async virtual Task DeleteAsync(TKey id)
    {
        await Repository.DeleteAsync(id);
    }

    public virtual async Task<ServiceResult<TGetOutputDto>> GetAsync(TKey id)
    {
        TEntity entity = await Repository.GetAsync(id);
        return ServiceResult<TGetOutputDto>.Successed(Mapper.Map<TGetOutputDto>(entity));
    }

    public virtual async Task<ServiceResult<TSimpleOutputDto>> UpdateAsync(TKey id, TUpdateInput updateInput)
    {
        TEntity entity = await GetEntityByIdAsync(id);
        Mapper.Map(updateInput, entity);
        await Repository.UpdateAsync(entity);
        return ServiceResult<TSimpleOutputDto>.Successed(Mapper.Map<TSimpleOutputDto>(entity));
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
