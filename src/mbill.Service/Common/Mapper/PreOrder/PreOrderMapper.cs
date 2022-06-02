namespace mbill.Service.Common.Mapper.Bill;

public class PreOrderMapper : Profile
{
    public PreOrderMapper()
    {
        CreateMap<CreatePreOrderInput, PreOrderEntity>();

        CreateMap<UpdatePreOrderInput, PreOrderEntity>();

        CreateMap<PreOrderEntity, PreOrderDto>();

        CreateMap<CreatePreOrderGroupInput, PreOrderGroupEntity>();

        CreateMap<UpdatePreOrderGroupInput, PreOrderGroupEntity>();

        CreateMap<PreOrderGroupEntity, PreOrderGroupDto>();
    }
}
