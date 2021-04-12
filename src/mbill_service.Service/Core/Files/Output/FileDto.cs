using mbill_service.Core.Domains.Common.Base;

namespace mbill_service.Service.Core.Files.Output
{
    public class FileDto : EntityDto
    {
        public string Key { get; set; }
        public string Path { get; set; }
        public string Url { get; set; }
    }
}
