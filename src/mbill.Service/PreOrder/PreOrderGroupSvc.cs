namespace mbill.Service.PreOrder;

public class PreOrderGroupSvc : CrudApplicationSvc<PreOrderGroupEntity, PreOrderGroupDto, long, CreatePreOrderGroupInput, UpdatePreOrderGroupInput>, IPreOrderGroupSvc
{
    private IPreOrderGroupRepo _groupRepo;

    public PreOrderGroupSvc(
        IAuditBaseRepo<PreOrderGroupEntity, long> repository,
        IPreOrderGroupRepo preOrderGroupRepo
        ) : base(repository)
    {
        _groupRepo = preOrderGroupRepo;
    }

    public override async Task<ServiceResult<PreOrderGroupDto>> CreateAsync(CreatePreOrderGroupInput input)
    {
        var exist = await _groupRepo.Select.AnyAsync(g => g.Name.Equals(input.Name));
        if (exist) return ServiceResult<PreOrderGroupDto>.Failed("已存在同名分组");
        return await base.CreateAsync(input);
    }
}
