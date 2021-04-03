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
        public IEnumerable<ChildGroupDto> ChildCategoryStas { get; set; }
    }

    public class ChildGroupDto 
    {
        public string ParentName { get; set; }

        public IEnumerable<object> Childs { get; set; }
    }

}
