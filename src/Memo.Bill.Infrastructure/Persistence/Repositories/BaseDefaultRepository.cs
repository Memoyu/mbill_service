using FreeSql;
using System.Linq.Expressions;
using MediatR;
using System.Reflection;
using System.Collections.Concurrent;
using Memo.Bill.Application.Common.Security;

namespace Memo.Bill.Infrastructure.Persistence.Repositories;

public class BaseDefaultRepository<TEntity> : DefaultRepository<TEntity, long>, IBaseDefaultRepository<TEntity> where TEntity : BaseAuditEntity
{
    private ConcurrentDictionary<Type, DbContext> _dicDbProp = new();

    private readonly IPublisher _publisher;

    /// <summary>
    ///  当前登录人信息
    /// </summary>
    protected readonly CurrentUser CurrentUser;

    public BaseDefaultRepository(UnitOfWorkManager unitOfWorkManager, ICurrentUserProvider currentUserProvider, IPublisher publisher) : base(unitOfWorkManager?.Orm, unitOfWorkManager)
    {
        CurrentUser = currentUserProvider.GetCurrentUser();
        _publisher = publisher;
    }


    private DbContext _db => _dicDbProp.GetOrAdd(
        EntityType, fn =>
        {
            var a = typeof(BaseRepository<,>).MakeGenericType(EntityType, typeof(long));
            // var ps = a.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic);
            var pb = a.GetProperty("_db", BindingFlags.Instance | BindingFlags.NonPublic);
            return pb?.GetValue(this) as DbContext ?? throw new Exception("RepositoryDbContext 为空");
        });

    private DbSet<TEntity> _dbset => _db.Set<TEntity>();

    protected void BeforeInsert(TEntity entity)
    {
        if (entity is BaseAuditEntity createAudit)
        {
            createAudit.CreateTime = DateTime.Now;
            createAudit.CreateUserId = CurrentUser.Id;
        }

        BeforeUpdate(entity);
    }

    protected void BeforeUpdate(TEntity entity)
    {
        if (entity is BaseAuditEntity updateAudit)
        {
            updateAudit.UpdateTime = DateTime.Now;
            updateAudit.UpdateUserId = CurrentUser.Id;
        }
    }

    protected void BeforeDelete(TEntity entity)
    {
        if (entity is BaseAuditEntity deleteAudit)
        {
            deleteAudit.IsDeleted = true;
            deleteAudit.DeleteUserId = CurrentUser.Id;
            deleteAudit.DeleteTime = DateTime.Now;
        }
    }

    #region 领域事件

    protected void PublishDomainEvents(TEntity entity)
    {
        if (entity is not BaseEntity domainEntity) return;

        var domainEvents = domainEntity.GetDomainEvents().ToList();

        domainEntity.ClearDomainEvents();

        foreach (var domainEvent in domainEvents)
            _publisher.Publish(domainEvent).ConfigureAwait(false).GetAwaiter();
    }

    protected async Task PublishDomainEventsAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity is not BaseEntity domainEntity) return;

        var domainEvents = domainEntity.GetDomainEvents().ToList();

        domainEntity.ClearDomainEvents();

        foreach (var domainEvent in domainEvents)
            await _publisher.Publish(domainEvent, cancellationToken);
    }

    #endregion

    public override ISelect<TEntity> Select => _dbset.Select;

    #region Insert

    public override TEntity Insert(TEntity entity)
    {
        BeforeInsert(entity);

        _dbset.Add(entity);

        PublishDomainEvents(entity);

        _db.SaveChanges();

        return entity;
    }

    public override async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        BeforeInsert(entity);

        await _dbset.AddAsync(entity, cancellationToken);

        await PublishDomainEventsAsync(entity, cancellationToken);

        await _db.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public override List<TEntity> Insert(IEnumerable<TEntity> entities)
    {
        foreach (TEntity entity in entities)
            BeforeInsert(entity);

        _dbset.AddRange(entities);

        foreach (var entity in entities)
            PublishDomainEvents(entity);

        _db.SaveChanges();

        return entities.ToList();
    }

    public override async Task<List<TEntity>> InsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        foreach (TEntity entity in entities)
            BeforeInsert(entity);

        await _dbset.AddRangeAsync(entities, cancellationToken);

        foreach (var entity in entities)
            await PublishDomainEventsAsync(entity, cancellationToken);

        await _db.SaveChangesAsync(cancellationToken);

        return entities.ToList(); ;
    }

    #endregion

    #region Update

    public override int Update(TEntity entity)
    {
        BeforeUpdate(entity);

        _dbset.Update(entity);

        PublishDomainEvents(entity);

        return _db.SaveChanges();
    }

    public override async Task<int> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        BeforeUpdate(entity);

        _dbset.Update(entity);

        await PublishDomainEventsAsync(entity, cancellationToken);

        return await _db.SaveChangesAsync(cancellationToken);
    }

    public override int Update(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
            BeforeUpdate(entity);

        _dbset.UpdateRange(entities);

        foreach (var entity in entities)
            PublishDomainEvents(entity);

        return _db.SaveChanges();
    }

    public override async Task<int> UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
            BeforeUpdate(entity);

        _dbset.UpdateRange(entities);

        foreach (var entity in entities)
            await PublishDomainEventsAsync(entity, cancellationToken);

        return await _db.SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region Delete

    public override int Delete(long id)
    {
        TEntity entity = Get(id);
        BeforeDelete(entity);

        // 由Update发布领域事件
        return Update(entity);
    }

    public override int Delete(TEntity entity)
    {
        _dbset.Attach(entity);
        BeforeDelete(entity);

        // 由Update发布领域事件
        return Update(entity);
    }

    public override int Delete(IEnumerable<TEntity> entities)
    {
        _dbset.AttachRange(entities);
        foreach (TEntity entity in entities)
            BeforeDelete(entity);

        // 由Update发布领域事件
        return Update(entities);
    }

    public override async Task<int> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        TEntity entity = await GetAsync(id, cancellationToken);
        BeforeDelete(entity);

        // 由Update发布领域事件
        return await UpdateAsync(entity, cancellationToken);
    }

    public override async Task<int> DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        _dbset.AttachRange(entities);

        foreach (TEntity entity in entities)
            BeforeDelete(entity);

        // 由Update发布领域事件
        return await UpdateAsync(entities, cancellationToken);
    }

    public override async Task<int> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbset.Attach(entity);
        BeforeDelete(entity);

        // 由Update发布领域事件
        return await UpdateAsync(entity, cancellationToken);
    }

    public override int Delete(Expression<Func<TEntity, bool>> predicate)
    {
        List<TEntity> items = base.Select.Where(predicate).ToList();
        if (items.Count == 0)
            return 0;

        foreach (var entity in items)
            BeforeDelete(entity);

        // 由Update发布领域事件
        return Update(items);
    }

    public override async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        List<TEntity> items = await base.Select.Where(predicate).ToListAsync(cancellationToken);
        if (items.Count == 0)
            return 0;

        foreach (var entity in items)
            BeforeDelete(entity);

        // 由Update发布领域事件
        return await UpdateAsync(items, cancellationToken);
    }

    #endregion

    #region InsertOrUpdate

    public override TEntity InsertOrUpdate(TEntity entity)
    {
        BeforeInsert(entity);

        _dbset.AddOrUpdate(entity);

        PublishDomainEvents(entity);

        _db.SaveChanges();

        return entity;
    }

    public override async Task<TEntity> InsertOrUpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        BeforeInsert(entity);

        await _dbset.AddOrUpdateAsync(entity, cancellationToken);

        await PublishDomainEventsAsync(entity);

        await _db.SaveChangesAsync(cancellationToken);

        return entity;
    }

    #endregion
}
