namespace mbill_service.Core.Common;

public class DomainReflexUtil
{
    /// <summary>
    /// 扫描 IEntity类所在程序集，反射得到类上有特性标签为TableAttribute 的所有类
    /// </summary>
    /// <returns></returns>
    public static Type[] GetTypesByTableAttribute()
    {
        List<Type> tableAssembies = new List<Type>();
        foreach (Type type in Assembly.GetAssembly(typeof(IEntity)).GetExportedTypes())
        {
            foreach (Attribute attribute in type.GetCustomAttributes())
            {
                if (attribute is TableAttribute tableAttribute)
                {
                    if (tableAttribute.DisableSyncStructure == false)
                    {
                        tableAssembies.Add(type);
                    }
                }
            }
        };
        return tableAssembies.ToArray();
    }

    /// <summary>
    /// 获取WebApi程序集下的所有Controller下的所有Action的权限信息
    /// </summary>
    /// <returns></returns>
    public static List<PermissionDefinition> GetAssemblyPermissionAttributes()
    {
        List<PermissionDefinition> permissions = new List<PermissionDefinition>();

        //获取WebApi程序集
        List<Type> assembly = Assembly.Load("mbill_service").GetTypes().AsEnumerable()
            .Where(type => typeof(ControllerBase).IsAssignableFrom(type)).ToList();

        //通过反射得到控制器上的权限特性标签
        assembly.ForEach(type =>
        {
            LocalAuthorizeAttribute permission = type.GetCustomAttribute<LocalAuthorizeAttribute>();
            RouteAttribute routerAttribute = type.GetCustomAttribute<RouteAttribute>();
            if (permission?.Permission != null && routerAttribute?.Template != null)
            {
                permissions.Add(new PermissionDefinition(permission.Permission, permission.Module, routerAttribute.Template));
            }
        });

        //得到方法上的权限特性标签，并排除无权限及模块的非固定权限
        assembly.ForEach(type =>
        {
            RouteAttribute routerAttribute = type.GetCustomAttribute<RouteAttribute>();//获取方法上的路由特性
            if (routerAttribute?.Template != null)
            {
                foreach (MethodInfo methodInfo in type.GetMethods())
                {
                    HttpMethodAttribute methodHttpAttribute = GetMethodHttpAttribute(methodInfo);//获取方法Http相关特性

                    foreach (Attribute attribute in methodInfo.GetCustomAttributes())//
                    {
                        if (attribute is LocalAuthorizeAttribute permission && !string.IsNullOrEmpty(permission.Permission) && !string.IsNullOrEmpty(permission.Module))
                        {
                            string actionHttpTemplate = methodHttpAttribute.Template != null ? "/" + methodHttpAttribute.Template + " " : " ";
                            string router = $"{routerAttribute.Template}{actionHttpTemplate}{methodHttpAttribute.HttpMethods.FirstOrDefault()}";//"路由模板"+"Http方法模板"+" "+"http方法"
                            permissions.Add(
                                    new PermissionDefinition(
                                            permission.Permission,
                                            permission.Module,
                                            router
                                        )
                                );
                        }
                    }
                }
            }
        });

        return permissions.Distinct().ToList();
    }

    /// <summary>
    /// 获取Method的http特性
    /// </summary>
    /// <param name="methodInfo">method信息</param>
    /// <returns></returns>
    private static HttpMethodAttribute GetMethodHttpAttribute(MethodInfo methodInfo)
    {
        HttpMethodAttribute methodAttribute = methodInfo.GetCustomAttributes().OfType<HttpGetAttribute>().FirstOrDefault();
        if (methodAttribute != null) return methodAttribute;

        methodAttribute = methodInfo.GetCustomAttributes().OfType<HttpDeleteAttribute>().FirstOrDefault();
        if (methodAttribute != null) return methodAttribute;

        methodAttribute = methodInfo.GetCustomAttributes().OfType<HttpPutAttribute>().FirstOrDefault();
        if (methodAttribute != null) return methodAttribute;

        methodAttribute = methodInfo.GetCustomAttributes().OfType<HttpPostAttribute>().FirstOrDefault();
        return methodAttribute;

    }
}
