namespace Mbill.Service.Core.User.Input
{
    public class ModifyUserBaseDto
    {
        public long Id { get; set; }

        /// <summary>
        /// 头像地址
        /// </summary>
        public string AvatarUrl { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [Required(ErrorMessage = "必须传入昵称")]
        public string Nickname { get; set; }

        /// <summary>
        /// 性别，0：未知，1：男，2：女
        /// </summary>
        [Required(ErrorMessage = "必须传入昵称")]
        public int Gender { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }
    }
}
