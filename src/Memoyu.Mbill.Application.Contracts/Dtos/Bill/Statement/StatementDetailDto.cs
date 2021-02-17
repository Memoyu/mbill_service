using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoyu.Mbill.Application.Contracts.Dtos.Bill.Statement
{
    public class StatementDetailDto : StatementDto
    {
        public string categoryParentName { get; set; }

        public string assetParentName { get; set; }
    }
}
