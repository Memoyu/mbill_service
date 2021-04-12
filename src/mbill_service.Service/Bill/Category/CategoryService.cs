using AutoMapper;
using mbill_service.Core.Domains.Common.Enums.Base;
using mbill_service.Core.Domains.Entities.Bill;
using mbill_service.Core.Exceptions;
using mbill_service.Core.Interface.IRepositories.Bill;
using mbill_service.Core.Interface.IRepositories.Core;
using mbill_service.Service.Base;
using mbill_service.Service.Bill.Category.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mbill_service.Service.Bill.Category
{
    public class CategoryService : ApplicationService, ICategoryService
    {
        private readonly ICategoryRepo _categoryRepo;
        private readonly IFileRepo _fileRepo;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepo categoryRepo, IFileRepo fileRepo, IMapper mapper)
        {
            _categoryRepo = categoryRepo;
            _fileRepo = fileRepo;
            _mapper = mapper;
        }
        public Task DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CategoryGroupDto>> GetGroupsAsync(string type)
        {
            List<CategoryEntity> entities = await _categoryRepo
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
                            s.IconUrl = _fileRepo.GetFileUrl(s.IconUrl);
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
           var category = await _categoryRepo.GetCategoryAsync(id) ?? throw new KnownException("分类信息不存在或已删除！", ServiceResultCode.NotFound);
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> GetParentAsync(long id)
        {
            var category = await _categoryRepo.GetCategoryAsync(id) ?? throw new KnownException("分类信息不存在或已删除！", ServiceResultCode.NotFound);
            var categoryParent = await _categoryRepo.GetCategoryAsync(category.ParentId) ?? throw new KnownException("分类父项信息不存在或已删除！", ServiceResultCode.NotFound);
            return _mapper.Map<CategoryDto>(categoryParent);
        }

        public async Task InsertAsync(CategoryEntity categroy)
        {
            if (!string.IsNullOrEmpty(categroy.Name))
            {
                bool isRepeatName = await _categoryRepo.Select.AnyAsync(r => r.Name == categroy.Name);
                if (isRepeatName)//分类名重复
                {
                    throw new KnownException("分类名称重复，请重新输入", ServiceResultCode.RepeatField);
                }
            }
            await _categoryRepo.InsertAsync(categroy);
        }

        public Task UpdateAsync(long id, CategoryEntity categroy)
        {
            throw new NotImplementedException();
        }
    }
}
