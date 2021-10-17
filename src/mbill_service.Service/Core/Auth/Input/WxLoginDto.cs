namespace mbill_service.Service.Core.Auth.Input
{
    public class WxLoginDto : BaseLoginDto
    {
        public string Nickname { get; set; }

        public int Gender { get; set; }

        public string Language { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public string Country { get; set; }

        public string AvatarUrl { get; set; }

        public string Code { get; set; }
    }
}
