using Mbill.Core.Domains.Common.Base;
using System.Linq.Expressions;
using System.Threading;

namespace Mbill.Infrastructure.Repository.Base
{
    public class AuditBaseRepo<TEntity> : AuditBaseRepo<TEntity, long>, IAuditBaseRepo<TEntity> where TEntity : class, new()
    {
        public AuditBaseRepo(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
        {
        }
    }

    public class AuditBaseRepo<TEntity, TKey> : DefaultRepository<TEntity, TKey>, IAuditBaseRepo<TEntity, TKey>
        where TEntity : class, new()
    {
        protected readonly ICurrentUser CurrentUser;
        public AuditBaseRepo(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager?.Orm, unitOfWorkManager)
        {
            CurrentUser = currentUser;
        }

        private void BeforeInsert(TEntity entity)
        {
            if (!(entity is ICreateAduitEntity e)) return;

            e.CreateTime = DateTime.Now;
            if (e.CreateUserBId == 0 && CurrentUser.BId != null)
            {
                e.CreateUserBId = CurrentUser.BId ?? 0;
            }

            if (!(entity is IUpdateAuditEntity updateAuditEntity)) return;
            updateAuditEntity.UpdateTime = DateTime.Now;
            updateAuditEntity.UpdateUserBId = CurrentUser.BId;
        }

        public override TEntity Insert(TEntity entity)
        {
            BeforeInsert(entity);
            return base.Insert(entity);
        }

        public override Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            this.BeforeInsert(entity);
            return base.InsertAsync(entity);
        }

        public override List<TEntity> Insert(IEnumerable<TEntity> entitys)
        {
            foreach (TEntity entity in entitys)
            {
                BeforeInsert(entity);
            }

            return base.Insert(entitys);
        }

        public override Task<List<TEntity>> InsertAsync(IEnumerable<TEntity> entitys, CancellationToken cancellationToken = default(CancellationToken))
        {
            var enumerable = entitys as TEntity[] ?? entitys.ToArray();
            foreach (TEntity entity in enumerable)
            {
                BeforeInsert(entity);
            }
            return base.InsertAsync(enumerable);
        }

        private void BeforeUpdate(TEntity entity)
        {
            if (!(entity is IUpdateAuditEntity e)) return;
            e.UpdateTime = DateTime.Now;
            e.UpdateUserBId = CurrentUser.BId;
        }

        public new int Update(TEntity entity)
        {
            BeforeUpdate(entity);
            return base.Update(entity);
        }

        public new Task<int> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            BeforeUpdate(entity);
            return base.UpdateAsync(entity);
        }

        public Task<int> UpdateWithIgnoreAsync(TEntity entity, Expression<Func<TEntity, object>> ignoreExp, CancellationToken cancellationToken = default(CancellationToken))
        {
            BeforeUpdate(entity);
            return Orm.Update<TEntity>().SetSource(entity).IgnoreColumns(ignoreExp).ExecuteAffrowsAsync();
        }

        public override int Update(IEnumerable<TEntity> entitys)
        {
            foreach (var entity in entitys)
            {
                this.BeforeUpdate(entity);
            }
            return base.Update(entitys);
        }

        public override Task<int> UpdateAsync(IEnumerable<TEntity> entitys, CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var entity in entitys)
            {
                BeforeUpdate(entity);
            }
            return base.UpdateAsync(entitys);
        }

        public override int Delete(TEntity entity)
        {
            if (entity is IDeleteAduitEntity)
            {
                return Orm.Update<TEntity>(entity)
                           .Set(a => (a as IDeleteAduitEntity).IsDeleted, true)
                           .Set(a => (a as IDeleteAduitEntity).DeleteUserBId, CurrentUser.BId)
                           .Set(a => (a as IDeleteAduitEntity).DeleteTime, DateTime.Now)
                           .ExecuteAffrows();
            }

            return base.Delete(entity);
        }

        public override int Delete(IEnumerable<TEntity> entitys)
        {
            if (entitys.Any())
            {
                Attach(entitys);
                foreach (TEntity x1 in entitys)
                {
                    if (x1 is IDeleteAduitEntity softDelete)
                    {
                        softDelete.DeleteUserBId = CurrentUser.BId;
                        softDelete.DeleteTime = DateTime.Now;
                        softDelete.IsDeleted = true;
                    }
                }

                return Update(entitys);
            }

            return base.Delete(entitys);
        }

        public override async Task<int> DeleteAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken))
        {
            TEntity entity = await base.GetAsync(id);
            if (entity is IDeleteAduitEntity)
            {
                return Orm.Update<TEntity>(entity)
                           .Set(a => (a as IDeleteAduitEntity).IsDeleted, true)
                           .Set(a => (a as IDeleteAduitEntity).DeleteUserBId, CurrentUser.BId)
                           .Set(a => (a as IDeleteAduitEntity).DeleteTime, DateTime.Now)
                           .ExecuteAffrows();
            }

            return await base.DeleteAsync(id);
        }


        public override Task<int> DeleteAsync(IEnumerable<TEntity> entitys, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (entitys.Any() && entitys.FirstOrDefault() is IDeleteAduitEntity)
            {
                Attach(entitys);
                foreach (TEntity x1 in entitys)
                {
                    if (x1 is IDeleteAduitEntity softDelete)
                    {
                        softDelete.DeleteUserBId = CurrentUser.BId;
                        softDelete.DeleteTime = DateTime.Now;
                        softDelete.IsDeleted = true;
                    }
                }
                return UpdateAsync(entitys);
            }
            return base.DeleteAsync(entitys);
        }

        public override async Task<int> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (entity is IDeleteAduitEntity)
            {
                return await Orm.Update<TEntity>(entity)
                    .Set(a => (a as IDeleteAduitEntity).IsDeleted, true)
                    .Set(a => (a as IDeleteAduitEntity).DeleteUserBId, CurrentUser.BId)
                    .Set(a => (a as IDeleteAduitEntity).DeleteTime, DateTime.Now)
                    .ExecuteAffrowsAsync();
            }

            return base.Delete(entity);
        }
        public override int Delete(Expression<Func<TEntity, bool>> predicate)
        {
            if (typeof(IDeleteAduitEntity).IsAssignableFrom(typeof(TEntity)))
            {
                List<TEntity> items = Orm.Select<TEntity>().Where(predicate).ToList();
                if (items.Count == 0)
                {
                    return 0;
                }
                return Orm.Update<TEntity>(items)
                    .Set(a => (a as IDeleteAduitEntity).IsDeleted, true)
                    .Set(a => (a as IDeleteAduitEntity).DeleteUserBId, CurrentUser.BId)
                    .Set(a => (a as IDeleteAduitEntity).DeleteTime, DateTime.Now)
                    .ExecuteAffrows();
            }

            return base.Delete(predicate);
        }

        public override async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (typeof(IDeleteAduitEntity).IsAssignableFrom(typeof(TEntity)))
            {
                List<TEntity> items = Orm.Select<TEntity>().Where(predicate).ToList();
                if (items.Count == 0)
                {
                    return 0;
                }
                return await Orm.Update<TEntity>(items)
                     .Set(a => (a as IDeleteAduitEntity).IsDeleted, true)
                     .Set(a => (a as IDeleteAduitEntity).DeleteUserBId, CurrentUser.BId)
                     .Set(a => (a as IDeleteAduitEntity).DeleteTime, DateTime.Now)
                     .ExecuteAffrowsAsync();
            }

            return await base.DeleteAsync(predicate);
        }
        public override async Task<TEntity> InsertOrUpdateAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            BeforeInsert(entity);
            BeforeUpdate(entity);
            await base.InsertOrUpdateAsync(entity);
            return entity;
        }

    }
}
