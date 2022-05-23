namespace mbill.Service.Base;

public interface ICrudApplicationSvc<TGetOutputDto, TGetListOutputDto, in TKey, in TGetListInput, in TCreateInput, in TUpdateInput>
    where TGetOutputDto : IEntityDto<TKey>
    where TGetListOutputDto : IEntityDto<TKey>
{
    Task<PagedDto<TGetListOutputDto>> GetListAsync(TGetListInput input);

    Task<TGetOutputDto> GetAsync(TKey id);

    Task<TGetOutputDto> CreateAsync(TCreateInput createInput);

    Task<TGetOutputDto> UpdateAsync(TKey id, TUpdateInput updateInput);

    Task DeleteAsync(TKey id);
}
