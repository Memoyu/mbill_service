/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.WebApi.Aop.Filter
*   文件名称 ：SwaggerDocumentFilter.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-09 18:14:48
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static Memoyu.Mbill.Domain.Shared.Const.SystemConst;

namespace Memoyu.Mbill.WebApi.Aop.Filter
{
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
                        Description = "账户操作",
                        ExternalDocs = new OpenApiExternalDocs { Description = "获取Token/RefreshToken" }
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
}
