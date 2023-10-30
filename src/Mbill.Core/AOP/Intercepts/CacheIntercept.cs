using EasyCaching.Core;

namespace Mbill.Core.AOP.Intercepts;

public class CacheIntercept : IInterceptor
{
    private readonly IEasyCachingProvider _cacheProvider;

    public CacheIntercept(IEasyCachingProvider provider)
    {
        _cacheProvider = provider;
    }

    public void Intercept(IInvocation invocation)
    {
        try
        {
            bool isEnable = Appsettings.CacheEnable;
            Type returnType = invocation.Method.ReturnType;//获取返回类型

            if (isEnable == false || returnType == typeof(void))//不开启缓存或返回类型为void时，直接执行
            {
                invocation.Proceed();
                return;
            }

            MethodInfo method = invocation.MethodInvocationTarget ?? invocation.Method;

            var cacheAttrObj = method.GetCustomAttributes(typeof(CacheableAttribute), false).FirstOrDefault();//获取当前方法的 CacheableAttribute

            if (cacheAttrObj is CacheableAttribute cacheAttr)//如果为CacheableAttribute
            {
                string cacheKey = GenerateCacheKey(cacheAttr.CacheKey, invocation);//获取自定义缓存键

                var cacheValue = _cacheProvider.Get<string>(cacheKey);//获取缓存
                if (cacheValue != null)//存在缓存数据
                {
                    Type[] resultTypes = returnType.GenericTypeArguments;//获取泛型类型的数组，例如Task<T1,T2,T3>中的T1/T2/T3

                    if (returnType == typeof(Task))//如果返回类型为Task
                    {
                        invocation.ReturnValue = InterceptAsync(cacheKey, (Task)invocation.ReturnValue);
                    }
                    else if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))//如果返回类型为Task<>
                    {
                        dynamic d = JsonConvert.DeserializeObject(cacheValue.Value, resultTypes.FirstOrDefault());//将缓存反序列化为resultTypes.FirstOrDefault()类型
                        invocation.ReturnValue = Task.FromResult(d);//赋值返回值
                    }
                    else//否则为同步方法
                    {
                        invocation.ReturnValue = JsonConvert.DeserializeObject(cacheValue.Value, returnType);//直接赋值返序列化，
                    }

                    return;
                }

                invocation.Proceed();

                if (returnType == typeof(Task))//如果返回类型为Task
                {
                    invocation.ReturnValue = InterceptAsync(cacheKey, (Task)invocation.ReturnValue);
                }
                else if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
                {
                    invocation.ReturnValue = InterceptAsync(cacheKey, (dynamic)invocation.ReturnValue);
                }
                else
                {
                    _cacheProvider.Set(cacheKey, JsonConvert.SerializeObject(invocation.ReturnValue), TimeSpan.FromSeconds(Appsettings.CacheExpire));
                }
                return;
            }

            invocation.Proceed();
        }
        catch (Exception)
        {
            throw;
        }
    }

    // 异步返回task
    private async Task InterceptAsync(string cacheKey, Task task)
    {
        await task.ConfigureAwait(false);//相当于await Task.Run(()=>{}).ConfigureAwait(false);防止死锁
    }

    // 异步返回Task<T>
    private async Task<T> InterceptAsync<T>(string cacheKey, Task<T> task)
    {
        T result = await task.ConfigureAwait(false);
        await _cacheProvider.SetAsync(cacheKey, JsonConvert.SerializeObject(result), TimeSpan.FromSeconds(Appsettings.CacheExpire));
        return result;
    }

    /// <summary>
    /// 获取规则生成的缓存Key
    /// </summary>
    /// <param name="cacheKey"></param>
    /// <param name="invocation"></param>
    /// <returns></returns>
    private string GenerateCacheKey(string cacheKey, IInvocation invocation)
    {
        string className = invocation.TargetType.Name;
        string methodName = invocation.Method.Name;
        List<object> methodArguments = invocation.Arguments.ToList();//方法的参数集合
        string param = string.Empty;
        if (methodArguments.Count > 0)
        {
            string serializeString = JsonConvert.SerializeObject(methodArguments, Formatting.Indented, new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Ignore
            });
            param = ":" + EncryptUtil.Encrypt(serializeString);
        }
        return string.Concat(cacheKey ?? $"{className}:{methodName}", param);//最终生成：class:method:md5hash
    }
}