﻿using System;

namespace mbill_service.Service.Bill.Statement.Output
{
    public class StatementExpendTrendDto
    {

        public string Name { get; set; }

        public decimal Data { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
