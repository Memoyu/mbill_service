using Memo.Bill.Application.Accounts.Commands;

namespace Memo.Bill.Application.Common.Mappings
{
    public class AccountRegister : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<UpdateAccountCommand, Account>()
             .Map(d => d.AccountId, s => s.AccountId)
             .Map(d => d.Name, s => s.Name)
             .Map(d => d.Icon, s => s.Icon)
             .Map(d => d.IsDefault, s => s.IsDefault)
             .Map(d => d.ParentId, s => s.ParentId);
        }
    }
}
