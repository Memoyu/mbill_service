/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Application.Contracts.Attributes
*   文件名称 ：CacheableAttribute.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 11:42:17
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using System;

namespace Memoyu.Mbill.Application.Contracts.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CacheableAttribute : Attribute
    {
        public CacheableAttribute()
        {
        }

        public CacheableAttribute(string cacheKey)
        {
            CacheKey = cacheKey;
        }

        public string CacheKey { get; set; }

    }
}
