/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Domain.Data
*   文件名称 ：CodeFirstExtension.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-10 13:37:58
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using FreeSql;
using Memoyu.Mbill.Domain.Entities.System;
using Memoyu.Mbill.Domain.Entities.User;
using Memoyu.Mbill.Domain.Shared.Const;
using Memoyu.Mbill.ToolKits.Utils;
using System;
using System.Collections.Generic;

namespace Memoyu.Mbill.Domain.Data
{
    /// <summary>
    /// 构建种子数据
    /// </summary>
    public static class CodeFirstExtension
    {
        public static ICodeFirst SeedData(this ICodeFirst fsql)
        {
            fsql.Entity<UserEntity>(e =>
            {
                e.HasData(new List<UserEntity>()
                {
                    new UserEntity()
                    {
                        Nickname="超级管理员",
                        Username="administrator",
                        CreateTime=DateTime.Now,
                        IsDeleted=false,
                        UserIdentitys=new List<UserIdentityEntity>()
                        {
                            new UserIdentityEntity(UserIdentityEntity.Password,"administrator",EncryptUtil.Encrypt("123456"),DateTime.Now)
                        },
                        UserRoles=new List<UserRoleEntity>()
                        {
                            new UserRoleEntity(1,SystemConst.Role.Administrator)
                        },
                    },
                    new UserEntity()
                    {
                        Nickname="管理员",
                        Username="admin",
                        CreateTime=DateTime.Now,
                        IsDeleted=false,
                        UserIdentitys=new List<UserIdentityEntity>()
                        {
                            new UserIdentityEntity(UserIdentityEntity.Password,"administrator",EncryptUtil.Encrypt("123456"),DateTime.Now)
                        },
                        UserRoles=new List<UserRoleEntity>()
                        {
                            new UserRoleEntity(2,SystemConst.Role.Admin)
                        },
                    }
                });
            })
            .Entity<RoleEntity>(e =>
            {
                e.HasData(new List<RoleEntity>()
                {
                    new RoleEntity(RoleEntity.Administrator,"超级管理员",true , 1),
                    new RoleEntity(RoleEntity.Admin,"普通管理员",true,1),
                    new RoleEntity(RoleEntity.User,"普通用户",true,1)
                });
            })
            .Entity<BaseTypeEntity>(e =>
            {
                e.HasData(new List<BaseTypeEntity>()
                {
                    new BaseTypeEntity("Statement.Type","账目类型",1)
                    {
                        CreateTime=DateTime.Now,IsDeleted=false,CreateUserId = 1,
                        BaseItems=new List<BaseItemEntity>()
                        {
                            new BaseItemEntity("0","支出",1,true,1){CreateUserId = 1,CreateTime=DateTime.Now,IsDeleted=false},
                            new BaseItemEntity("1","收入",2,true,1){CreateUserId = 1,CreateTime=DateTime.Now,IsDeleted=false},
                            new BaseItemEntity("2","转账",3,true,1){CreateUserId = 1,CreateTime=DateTime.Now,IsDeleted=false},
                            new BaseItemEntity("3","还款",3,true,1){CreateUserId = 1,CreateTime=DateTime.Now,IsDeleted=false}
                        }
                    },
                        new BaseTypeEntity("Sex","性别",2)
                        {
                            CreateTime=DateTime.Now,IsDeleted=false,CreateUserId = 1,
                            BaseItems=new List<BaseItemEntity>()
                            {
                                new BaseItemEntity("0","未知",1,true,2){CreateTime=DateTime.Now,IsDeleted=false},
                                new BaseItemEntity("1","男",2,true,2){CreateTime=DateTime.Now,IsDeleted=false},
                                new BaseItemEntity("2","女",3,true,2){CreateTime=DateTime.Now,IsDeleted=false}
                            }
                        },
                });
            });
            return fsql;
        }
    }
}
