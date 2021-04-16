using mbill_service.Core.Domains.Common;
using System;

namespace mbill_service.Service.Core.Logger.Input
{
    public class LogPagingDto : PagingDto
    {
        public string Method { get; set; }
        public string Username { get; set; }
        public long UserId { get; set; }
        public int StatusCode { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}
