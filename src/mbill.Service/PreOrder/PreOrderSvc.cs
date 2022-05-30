namespace mbill.Service.PreOrder;

public class PreOrderSvc : CrudApplicationSvc<PreOrderEntity, PreOrderDto, long, CreatePreOrderInput, UpdatePreOrderInput>, IPreOrderSvc
{
    public PreOrderSvc(
        IAuditBaseRepo<PreOrderEntity, long> repository,
        IPreOrderRepo preOrderRepo,
        IPreOrderGroupRepo preOrderGroupRepo
        ) : base(repository)
    {
    }
}
