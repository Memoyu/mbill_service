namespace mbill.Service.Base;

public interface ICrudApplicationSvc<TGetOutputDto, in TKey, in TCreateInput, in TUpdateInput>
    where TGetOutputDto : IEntityDto<TKey>
{
    Task<TGetOutputDto> GetAsync(TKey id);

    Task<TGetOutputDto> CreateAsync(TCreateInput createInput);

    Task<TGetOutputDto> UpdateAsync(TKey id, TUpdateInput updateInput);

    Task DeleteAsync(TKey id);
}
