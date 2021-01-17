/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Application.Contracts.Mapper.Converter
*   文件名称 ：GenderFormatter.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-17 14:57:15
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using AutoMapper;
using Memoyu.Mbill.Domain.Base;
using Memoyu.Mbill.Domain.Entities.Core;

namespace Memoyu.Mbill.Application.Contracts.Mapper.Converter
{
    public class GenderFormatter : IValueConverter<int, string>
    {
        private readonly IAuditBaseRepository<BaseTypeEntity> _baseTypeRepository;
        private readonly IAuditBaseRepository<BaseItemEntity> _baseItemRepository;
        public GenderFormatter(IAuditBaseRepository<BaseItemEntity> baseItemRepository, IAuditBaseRepository<BaseTypeEntity> baseTypeRepository)
        {
            _baseItemRepository = baseItemRepository;
            _baseTypeRepository = baseTypeRepository;
        }
        public string Convert(int sourceMember, ResolutionContext context)
        {
            var typeId = _baseTypeRepository.Select.Where(t => t.TypeCode == "Sex").ToOne()?.Id;
            var item = _baseItemRepository.Select.Where(i => i.BaseTypeId == typeId && i.ItemCode == $"{sourceMember}").ToOne();
            return item?.ItemName;
        }
    }
}
