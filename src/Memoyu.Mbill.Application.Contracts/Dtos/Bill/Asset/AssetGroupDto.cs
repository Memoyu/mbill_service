/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Application.Contracts.Dtos.Bill.Asset
*   文件名称 ：AssetGroupDto.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-23 10:21:50
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using System.Collections.Generic;

namespace Memoyu.Mbill.Application.Contracts.Dtos.Bill.Asset
{
    public class AssetGroupDto
    {
        public string Name { get; set; }

        public List<AssetDto> Childs { get; set; }
    }
}
