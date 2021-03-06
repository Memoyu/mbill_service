﻿using System.Collections.Generic;

namespace mbill_service.Service.Bill.Statement.Output
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
