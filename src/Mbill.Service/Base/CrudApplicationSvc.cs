using Mbill.Core.Common;

namespace Mbill.Service.Base;

public abstract class CrudApplicationSvc<TEntity, TGetOutputDto, TSimpleOutputDto, TCreateInput, TUpdateInput>
        : ApplicationSvc, ICrudApplicationSvc<TGetOutputDto, TSimpleOutputDto, TCreateInput, TUpdateInput>
        where TEntity : class, IEntity
        where TGetOutputDto : IEntityDto
        where TUpdateInput : BaseUpdateInput
{
    /// <summary>
    /// 仓储
    /// </summary>
    protected IAuditBaseRepo<TEntity, long> Repository { get; }

    protected CrudApplicationSvc(IAuditBaseRepo<TEntity, long> repository)
    {
        Repository = repository;
    }


    public async virtual Task<ServiceResult<TSimpleOutputDto>> CreateAsync(TCreateInput createInput)
    {
        TEntity entity = Mapper.Map<TEntity>(createInput);
        entity.BId = SnowFlake.NextId();
        await Repository.InsertAsync(entity);
        return ServiceResult<TSimpleOutputDto>.Successed(Mapper.Map<TSimpleOutputDto>(entity));
    }

    public async virtual Task<ServiceResult> DeleteAsync(long bId)
    {
        TEntity entity = await GetEntityByIdAsync(bId);
        if (entity is null) return ServiceResult.Failed("要删除的数据不存在");
        await Repository.DeleteAsync(entity);
        return ServiceResult.Successed();
    }

    public virtual async Task<ServiceResult<TGetOutputDto>> GetAsync(long bId)
    {
        TEntity entity = await GetEntityByIdAsync(bId);
        return ServiceResult<TGetOutputDto>.Successed(Mapper.Map<TGetOutputDto>(entity));
    }

    public virtual async Task<ServiceResult<TSimpleOutputDto>> UpdateAsync(TUpdateInput updateInput)
    {
        TEntity entity = await GetEntityByIdAsync(updateInput.BId);
        if (entity is null) return ServiceResult<TSimpleOutputDto>.Failed("要更新的数据不存在");
        Mapper.Map(updateInput, entity);
        await Repository.UpdateAsync(entity);
        return ServiceResult<TSimpleOutputDto>.Successed(Mapper.Map<TSimpleOutputDto>(entity));
    }

    protected virtual ISelect<TEntity> QueryAll()
    {
        return Repository.Select;
    }
    protected virtual async Task<TEntity> GetEntityByIdAsync(long bId)
    {
        return await Repository.Select.Where(e => e.BId == bId).ToOneAsync();
    }
}
