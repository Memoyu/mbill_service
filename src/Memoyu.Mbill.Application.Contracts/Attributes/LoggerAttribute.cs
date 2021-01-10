/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Application.Contracts.Attributes
*   文件名称 ：LoggerAttribute.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-10 13:59:08
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using System;

namespace Memoyu.Mbill.Application.Contracts.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class LoggerAttribute : Attribute
    {
        public string Template { get; }

        public LoggerAttribute(string template)
        {
            Template = template ?? throw new ArgumentNullException(nameof(template));
        }
    }
}
