using Memo.Bill.Application.Accounts.Commands;

namespace Memo.Bill.Application.Common.Mappings
{
    public class AccountRegister : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<UpdateAccountCommand, Account>().IgnoreNullValues(true);

        }
    }
}
