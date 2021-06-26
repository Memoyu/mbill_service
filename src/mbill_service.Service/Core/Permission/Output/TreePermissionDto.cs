using System;
using System.Collections.Generic;

namespace mbill_service.Service.Core.Permission.Output
{
    public class TreePermissionDto
    {
        public long Id { get; set; }
        public string Rowkey { get; set; }
        public string Name { get; set; }
        public string Router { get; set; }
        public DateTime? CreateTime { get; set; }
        public List<TreePermissionDto> Children { get; set; }
    }
}
