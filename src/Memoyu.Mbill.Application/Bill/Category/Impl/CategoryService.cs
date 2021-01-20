/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Application.Bill.Category.Impl
*   文件名称 ：CategoryService.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-06 21:15:14
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Memoyu.Mbill.Application.Base.Impl;
using Memoyu.Mbill.Application.Contracts.Dtos.Bill.Category;
using Memoyu.Mbill.Application.Contracts.Exceptions;
using Memoyu.Mbill.Domain.Base;
using Memoyu.Mbill.Domain.Entities.Bill.Category;
using Memoyu.Mbill.ToolKits.Base.Enum.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoyu.Mbill.Application.Bill.Category.Impl
{
    public class CategoryService : ApplicationService, ICategoryService
    {
        private readonly IAuditBaseRepository<CategoryEntity, long> _categoryRepository;
        public CategoryService(IAuditBaseRepository<CategoryEntity , long> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public Task DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<CategoryDto> GetAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CategoryDto>> GetClassificationAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CategoryDto>> GetListAsync()
        {
            throw new NotImplementedException();
        }

        public async Task InsertAsync(CategoryEntity categroy)
        {
            if (!string.IsNullOrEmpty(categroy.Name))
            {
                bool isRepeatName = await _categoryRepository.Select.AnyAsync(r => r.Name == categroy.Name);
                if (isRepeatName)//分类名重复
                {
                    throw new KnownException("分类名称重复，请重新输入", ServiceResultCode.RepeatField);
                }
            }
            await _categoryRepository.InsertAsync(categroy);
        }

        public Task UpdateAsync(long id, CategoryEntity categroy)
        {
            throw new NotImplementedException();
        }
    }
}
