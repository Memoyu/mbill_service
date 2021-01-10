/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Domain.Shared.Security
*   文件名称 ：PermissionDefinition.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-09 15:01:48
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using System;

namespace Memoyu.Mbill.Domain.Shared.Security
{
    public class PermissionDefinition
    {
        public PermissionDefinition(string permission, string module, string router)
        {
            Permission = permission ?? throw new ArgumentNullException(nameof(permission));
            Module = module ?? throw new ArgumentNullException(nameof(module));
            Router = router ?? throw new ArgumentNullException(nameof(router));
        }

        public string Permission { get; }
        public string Module { get; }
        public string Router { get; }
        public override string ToString()
        {
            return base.ToString() + $" Permission:{Permission}、Module:{Module}、Router:{Router}";
        }
    }
}
