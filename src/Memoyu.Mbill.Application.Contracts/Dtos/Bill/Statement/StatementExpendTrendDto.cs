using System;
using System.Collections.Generic;

namespace Memoyu.Mbill.Application.Contracts.Dtos.Bill.Statement
{
    public class StatementExpendTrendDto
    {

        public string Name { get; set; }

        public decimal Data { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
