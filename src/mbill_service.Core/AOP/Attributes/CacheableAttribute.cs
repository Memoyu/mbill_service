using System;

namespace mbill_service.Core.AOP.Attributes
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
