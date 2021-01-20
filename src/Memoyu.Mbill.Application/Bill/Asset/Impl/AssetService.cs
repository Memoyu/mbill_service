/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Application.Bill.Asset.Impl
*   文件名称 ：AssetService.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-06 21:06:06
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Memoyu.Mbill.Application.Base.Impl;
using Memoyu.Mbill.Application.Contracts.Exceptions;
using Memoyu.Mbill.Domain.Base;
using Memoyu.Mbill.Domain.Entities.Bill.Asset;
using Memoyu.Mbill.ToolKits.Base.Enum.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoyu.Mbill.Application.Bill.Asset.Impl
{
    public class AssetService : ApplicationService, IAssetService
    {
        private readonly IAuditBaseRepository<AssetEntity, long> _assetRepository;

        public AssetService(IAuditBaseRepository<AssetEntity , long> assetRepository)
        {
            _assetRepository = assetRepository;
        }
        public async Task InsertAsync(AssetEntity asset)
        {
            if (!string.IsNullOrEmpty(asset.Name))
            {
                bool isRepeatName = await _assetRepository.Select.AnyAsync(r => r.Name == asset.Name);
                if (isRepeatName)//资产分类名重复
                {
                    throw new KnownException("资产分类名称重复，请重新输入", ServiceResultCode.RepeatField);
                }
            }
            await _assetRepository.InsertAsync(asset);
        }
    }
}
