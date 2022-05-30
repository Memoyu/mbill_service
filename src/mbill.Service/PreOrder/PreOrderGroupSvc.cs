namespace mbill.Service.PreOrder;

public class PreOrderGroupSvc : CrudApplicationSvc<PreOrderGroupEntity, PreOrderDto, long, CreatePreOrderInput, UpdatePreOrderInput>, IPreOrderGroupSvc
{
    public PreOrderGroupSvc(
        IAuditBaseRepo<PreOrderGroupEntity, long> repository,
        IPreOrderGroupRepo preOrderGroupRepo
        ) : base(repository)
    {
    }
}
