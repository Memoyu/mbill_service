namespace Mbill.Service.Base;

public interface ICrudApplicationSvc<TGetOutputDto, TSimpleOutputDto, in TCreateInput, in TUpdateInput>
    where TGetOutputDto : IEntityDto
    where TUpdateInput : BaseUpdateInput
{
    Task<ServiceResult<TGetOutputDto>> GetAsync(long bId);

    Task<ServiceResult<TSimpleOutputDto>> CreateAsync(TCreateInput createInput);

    Task<ServiceResult<TSimpleOutputDto>> UpdateAsync(TUpdateInput updateInput);

    Task<ServiceResult> DeleteAsync(long bId);
}
