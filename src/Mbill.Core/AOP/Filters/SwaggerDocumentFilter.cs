namespace Mbill.Core.AOP.Filters;

public class SwaggerDocumentFilter : IDocumentFilter
{
    /// <summary>
    /// 对应Controller的API文档描述信息
    /// </summary>
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var tags = new List<OpenApiTag>
            {

                    new OpenApiTag
                    {
                        Name = "Account",
                        Description = "账户认证授权",
                        ExternalDocs = new OpenApiExternalDocs { Description = "账户认证授权" }
                    },
                    new OpenApiTag
                    {
                        Name = "User",
                        Description = "用户相关接口",
                        ExternalDocs = new OpenApiExternalDocs { Description = "用户管理" }
                    },
            };

        #region 实现自定义API描述，并过滤不属于当前分组的API
        // 当前分组名称
        var groupName = context.ApiDescriptions.FirstOrDefault()?.GroupName;

        // 当前所有的API对象
        var apis = context.ApiDescriptions.GetType().GetField("_source", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(context.ApiDescriptions) as IEnumerable<ApiDescription>;

        // 不属于当前分组的所有Controller
        // 注意：配置的OpenApiTag，Name名称要与Controller的Name对应才会生效。
        var controllers = apis.Where(x => x.GroupName != groupName).Select(x => ((ControllerActionDescriptor)x.ActionDescriptor).ControllerName).Distinct();

        // 筛选一下tags
        swaggerDoc.Tags = tags.Where(x => !controllers.Contains(x.Name)).OrderBy(x => x.Name).ToList();
        #endregion
    }
}
