using mbill_service.Core.AOP.Attributes;
using mbill_service.Core.Domains.Common;
using mbill_service.Core.Domains.Common.Enums.Base;
using mbill_service.Core.Domains.Entities.Core;
using mbill_service.Core.Domains.Entities.User;
using mbill_service.Core.Exceptions;
using mbill_service.Core.Extensions;
using mbill_service.Core.Interface.IRepositories.Core;
using mbill_service.Service.Base;
using mbill_service.Service.Core.User;
using mbill_service.Service.Core.User.Input;
using mbill_service.Service.Core.User.Output;
using mbill_service.ToolKits.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mbill_service.Service.Core.User
{
    public class UserSvc : ApplicationSvc, IUserSvc
    {
        private readonly IUserRepo _userRepo;
        private readonly IFileRepo _fileRepo;

        public UserSvc(IUserRepo userRepo, IFileRepo fileRepo)
        {
            _userRepo = userRepo;
            _fileRepo = fileRepo;
        }

        [Transactional]
        public async Task CreateAsync(UserEntity user, List<long> roleIds, string password)
        {
            if (!string.IsNullOrEmpty(user.Username))
            {
                bool isRepeatName = await _userRepo.Select.AnyAsync(r => r.Username == user.Username);
                if (isRepeatName)//用户名重复
                {
                    throw new KnownException("用户名重复，请重新输入", ServiceResultCode.RepeatField);
                }
            }

            if (!string.IsNullOrEmpty(user.Email.Trim()))
            {
                var isRepeatEmail = await _userRepo.Select.AnyAsync(r => r.Email == user.Email.Trim());
                if (isRepeatEmail)//邮箱重复
                {
                    throw new KnownException("注册邮箱重复，请重新输入", ServiceResultCode.RepeatField);
                }
            }
            user.UserRoles = new List<UserRoleEntity>();
            roleIds?.ForEach(roleId =>//遍历构建赋值角色
            {
                user.UserRoles.Add(new UserRoleEntity()
                {
                    RoleId = roleId
                });
            });

            user.UserIdentitys = new List<UserIdentityEntity>()//构建赋值用户身份认证登录信息
            {
                new UserIdentityEntity(UserIdentityEntity.Password,user.Username,EncryptUtil.Encrypt(password), DateTime.Now)
            };
            await _userRepo.InsertAsync(user);
        }

        public async Task<UserDto> GetAsync(long? id)
        {
            var userId = id ?? CurrentUser.Id;
            var user = await _userRepo
                .Select
                .IncludeMany(u => u.Roles)
                .Where(r => r.Id == userId).FirstAsync();
            user.AvatarUrl = _fileRepo.GetFileUrl(user.AvatarUrl);
            return Mapper.Map<UserDto>(user);
        }

        public async Task<PagedDto<UserDto>> GetPagesAsync(UserPagingDto pagingDto)
        {
            if (pagingDto.CreateStartTime != null && pagingDto.CreateEndTime == null) throw new KnownException("创建时间参数有误", ServiceResultCode.ParameterError);
            pagingDto.Sort = pagingDto.Sort.IsNullOrEmpty() ? "id ASC" : pagingDto.Sort.Replace("-", " ");
            bool? isEnable = pagingDto.IsEnable switch
            {
                1 => true,
                0 => false,
                _ => null
            };
            var users = await _userRepo
                .Select
                .IncludeMany(u => u.Roles)
                .WhereIf(!string.IsNullOrWhiteSpace(pagingDto.Username), u => u.Username.Contains(pagingDto.Username))
                .WhereIf(!string.IsNullOrWhiteSpace(pagingDto.Nickname), u => u.Nickname.Contains(pagingDto.Nickname))
                .WhereIf(isEnable != null, u => u.IsEnable == isEnable)
                .WhereIf(pagingDto.CreateStartTime != null, a => a.CreateTime >= pagingDto.CreateStartTime && a.CreateTime <= pagingDto.CreateEndTime)
                .WhereIf(pagingDto.RoleId > 0, u => u.Roles.AsSelect().Any(r => r.Id == pagingDto.RoleId))
                .OrderBy(pagingDto.Sort)
                .ToPageListAsync(pagingDto, out long totalCount);
            var userDtos = users.Select(u =>
             {
                 var dto = Mapper.Map<UserDto>(u);
                 dto.AvatarUrl = _fileRepo.GetFileUrl(dto.AvatarUrl);
                 return dto;
             }).ToList();

            return new PagedDto<UserDto>(userDtos, totalCount);
        }

        public Task UpdateAsync(long id, UserEntity inputDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }
    }
}
