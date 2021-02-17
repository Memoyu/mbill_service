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
using AutoMapper;
using Memoyu.Mbill.Application.Base.Impl;
using Memoyu.Mbill.Application.Contracts.Dtos.Bill.Category;
using Memoyu.Mbill.Application.Contracts.Exceptions;
using Memoyu.Mbill.Domain.Base;
using Memoyu.Mbill.Domain.Entities.Bill.Category;
using Memoyu.Mbill.Domain.IRepositories.Bill.Category;
using Memoyu.Mbill.Domain.IRepositories.Core;
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
        private readonly ICategoryRepository _categoryRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IFileRepository fileRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _fileRepository = fileRepository;
            _mapper = mapper;
        }
        public Task DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CategoryGroupDto>> GetGroupsAsync(string type)
        {
            List<CategoryEntity> entities = await _categoryRepository
                .Select
                .Where(c => c.IsDeleted == false)
                .WhereIf(type.IsNotNullOrEmpty(), c => c.Type.Equals(type))
                .ToListAsync();
            List<CategoryEntity> parents = entities.FindAll(c => c.ParentId == 0);
            List<CategoryGroupDto> dtos = parents
                .Select(c =>
                {
                    var dto = new CategoryGroupDto();
                    dto.Name = c.Name;
                    dto.Childs = entities
                        .FindAll(d => d.ParentId == c.Id)
                        .Select(d =>
                        {
                            var s = Mapper.Map<CategoryDto>(d);
                            s.IconUrl = _fileRepository.GetFileUrl(s.IconUrl);
                            return s;
                        })
                        .ToList();
                    return dto;
                })
                .ToList();
            return dtos;
        }

        public async Task<IEnumerable<CategoryDto>> GetListAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<CategoryDto> GetAsync(long id)
        {
           var category = await _categoryRepository.GetCategoryAsync(id) ?? throw new KnownException("分类信息不存在或已删除！", ServiceResultCode.NotFound);
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> GetParentAsync(long id)
        {
            var category = await _categoryRepository.GetCategoryAsync(id) ?? throw new KnownException("分类信息不存在或已删除！", ServiceResultCode.NotFound);
            var categoryParent = await _categoryRepository.GetCategoryAsync(category.ParentId) ?? throw new KnownException("分类父项信息不存在或已删除！", ServiceResultCode.NotFound);
            return _mapper.Map<CategoryDto>(categoryParent);
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
