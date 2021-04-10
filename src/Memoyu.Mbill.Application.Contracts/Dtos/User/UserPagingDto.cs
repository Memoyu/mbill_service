using Memoyu.Mbill.ToolKits.Base.Page;
using System;
using System.Collections.Generic;

namespace Memoyu.Mbill.Application.Contracts.Dtos.User
{
    public class UserPagingDto : PagingDto
    {
        public string Username { get; set; }

        public string Nickname { get; set; }

        public int IsEnable { get; set; }

        public long RoleId { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
