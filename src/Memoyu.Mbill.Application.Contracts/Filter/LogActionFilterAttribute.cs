/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Application.Contracts.Filter
*   文件名称 ：LogActionFilterAttribute.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-10 13:59:08
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using System.Linq;
using System.Text.RegularExpressions;
using Memoyu.Mbill.Domain.Base;
using Memoyu.Mbill.Domain.Shared.Security;
using Memoyu.Mbill.Domain.Entities.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Serilog;
using Memoyu.Mbill.Application.Contracts.Attributes;

namespace Memoyu.Mbill.Application.Contracts.Filter
{
    /// <summary>
    /// 全局日志记录
    /// </summary>
    public class LogActionFilterAttribute : ActionFilterAttribute
    {
        private readonly ICurrentUser _currentUser;
        private readonly IDiagnosticContext _diagnosticContext;
        private readonly IAuditBaseRepository<LogEntity> _logRepository;

        Regex regex = new Regex("(?<=\\{)[^}]*(?=\\})");

        public LogActionFilterAttribute(ICurrentUser currentUser, IDiagnosticContext diagnosticContext, IAuditBaseRepository<LogEntity> logRepository)
        {
            _currentUser = currentUser;
            _diagnosticContext = diagnosticContext;
            _logRepository = logRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _diagnosticContext.Set("ActionArguments", JsonConvert.SerializeObject(context.ActionArguments));
            _diagnosticContext.Set("RouteData", context.ActionDescriptor.RouteValues);
            _diagnosticContext.Set("ActionName", context.ActionDescriptor.DisplayName);
            _diagnosticContext.Set("ActionId", context.ActionDescriptor.Id);
            _diagnosticContext.Set("ValidationState", context.ModelState.IsValid);

            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            //当方法或控制器上存在DisableAuditingAttribute特性标签时，不记录日志 
            if (context.ActionDescriptor is ControllerActionDescriptor d &&
                d.MethodInfo.IsDefined(typeof(DisableAuditingAttribute), true) ||
                context.Controller.GetType().IsDefined(typeof(DisableAuditingAttribute), true))
            {
                base.OnActionExecuted(context);
                return;
            }
            LoggerAttribute loggerAttribute = context.ActionDescriptor.EndpointMetadata.OfType<LoggerAttribute>().FirstOrDefault();//获取日志模板特性
            var logEntity = new LogEntity//构建日志
            {
                Path = context.HttpContext.Request.Path,
                Method = context.HttpContext.Request.Method,
                StatusCode = context.HttpContext.Response.StatusCode,
                UserId = _currentUser.Id ?? 0,
                Username = _currentUser.UserName
            };

            if (loggerAttribute != null)//日志模板不为空
            {
                logEntity.Message = parseTemplate(loggerAttribute.Template, _currentUser, context.HttpContext.Request, context.HttpContext.Response);
            }
            else
            {
                logEntity.Message = $"访问{logEntity.Path}";
            }

            PermissionAuthorizeAttribute perAuthorize = context.ActionDescriptor.EndpointMetadata.OfType<PermissionAuthorizeAttribute>().FirstOrDefault();
            if (perAuthorize != null)//如果需要权限
            {
                logEntity.Authority = $"{perAuthorize.Module}  {perAuthorize.Permission}";
            }

            _logRepository.Insert(logEntity);

            base.OnActionExecuted(context);
        }

        /// <summary>
        /// 转换成模板
        /// </summary>
        /// <param name="template">模板</param>
        /// <param name="userInfo">当前用户信息</param>
        /// <param name="request">请求上下文</param>
        /// <param name="response">响应上下文</param>
        /// <returns></returns>
        private string parseTemplate(string template, ICurrentUser userInfo, HttpRequest request, HttpResponse response)
        {
            foreach (Match item in regex.Matches(template))
            {
                string propertyValue = extractProperty(item.Value, userInfo, request, response);
                template = template.Replace("{" + item.Value + "}", propertyValue);
            }
            return template;
        }

        /// <summary>
        /// 获取模板中属性对应的值
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userInfo"></param>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        private string extractProperty(string item, ICurrentUser userInfo, HttpRequest request, HttpResponse response)
        {
            int i = item.LastIndexOf('.');//例如user.Username,分割后的user与Username
            string obj = item.Substring(0, i);//user
            string prop = item.Substring(i + 1);//Username
            switch (obj)
            {
                case "user":
                    return getValueByPropName(userInfo, prop);
                case "request":
                    return getValueByPropName(request, prop);
                case "response":
                    return getValueByPropName(response, prop);
                default:
                    return "";
            }
        }

        /// <summary>
        /// 反射获取属性值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        private string getValueByPropName<T>(T t, string prop)
        {
            return t.GetType().GetProperty(prop)?.GetValue(t, null)?.ToString();
        }
    }
}