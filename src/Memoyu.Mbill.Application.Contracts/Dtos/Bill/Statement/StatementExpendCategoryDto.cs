using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoyu.Mbill.Application.Contracts.Dtos.Bill.Statement
{
    public class StatementExpendCategoryDto
    {
        public IEnumerable<StatisticsDto> ParentCategoryStas { get; set; }
        public IEnumerable<StatisticsDto> ChildCategoryStas { get; set; }
    }
}
