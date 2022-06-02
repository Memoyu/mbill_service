namespace mbill.Service.Base;

public interface ICrudApplicationSvc<TGetOutputDto, in TKey, in TCreateInput, in TUpdateInput>
    where TGetOutputDto : IEntityDto<TKey>
{
    Task<ServiceResult<TGetOutputDto>> GetAsync(TKey id);

    Task<ServiceResult<TGetOutputDto>> CreateAsync(TCreateInput createInput);

    Task<ServiceResult<TGetOutputDto>> UpdateAsync(TKey id, TUpdateInput updateInput);

    Task DeleteAsync(TKey id);
}
