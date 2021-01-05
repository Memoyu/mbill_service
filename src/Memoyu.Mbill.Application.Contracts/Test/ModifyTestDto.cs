/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Application.Contracts.Test
*   文件名称 ：ModifyTestDto.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 8:43:32
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Memoyu.Mbill.Application.Contracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace Memoyu.Mbill.Application.Contracts.Test
{
    public class ModifyTestDto
    {
        [Required(ErrorMessage = "必须传入姓名")]
        public string Name { get; set; }
        [EnumDataType(typeof(SexEnum) , ErrorMessage ="性别应为1(男)，2(女)")]
        public SexEnum Sex { get; set; }
        [Range(1,200,ErrorMessage ="年龄应该在1-200之间")]
        public int Age { get; set; }
        [StringLength(100, ErrorMessage = "地址应小于100字符")]
        public string Address { get; set; }
    }
}
