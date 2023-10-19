namespace mbill.Service.Base;

public interface ICrudApplicationSvc<TGetOutputDto, TSimpleOutputDto, in TKey, in TCreateInput, in TUpdateInput>
    where TGetOutputDto : IEntityDto
{
    Task<ServiceResult<TGetOutputDto>> GetAsync(TKey id);

    Task<ServiceResult<TSimpleOutputDto>> CreateAsync(TCreateInput createInput);

    Task<ServiceResult<TSimpleOutputDto>> UpdateAsync(TKey id, TUpdateInput updateInput);

    Task DeleteAsync(TKey id);
}
