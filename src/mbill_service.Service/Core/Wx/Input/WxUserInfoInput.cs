namespace mbill_service.Service.Core.Wx.Input
{
    public class WxUserInfoInput
    {
        public string Nickname { get; set; }

        public int Gender { get; set; }

        public string Language { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public string Country { get; set; }

        public string AvatarUrl { get; set; }

        public string OpenId { get; set; }

        public string UnionId { get; set; }
    }
}
