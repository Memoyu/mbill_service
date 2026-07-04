using Memo.Bill.Application.Icons.Common;

namespace Memo.Bill.Application.Icons.Queries;

[Authorize(Permissions = ApiPermission.Icon.ListGroup)]
public record ListCatalogIconQuery() : IAuthorizeableRequest<Result>;

public class ListCatalogIconQueryHandler(
    IBaseDefaultRepository<Icon> iconRepo,
    IBaseDefaultRepository<IconCatalog> iconCatalogRepo
    ) : IRequestHandler<ListCatalogIconQuery, Result>
{
    public async Task<Result> Handle(ListCatalogIconQuery request, CancellationToken cancellationToken)
    {
        var catalogs = await iconCatalogRepo.Select.ToListAsync(cancellationToken);
        var icons = await iconRepo.Select.ToListAsync(cancellationToken);

        var result = new List<IconCatalogResult>();
        foreach (var catalog in catalogs)
        {
            result.Add(new IconCatalogResult
            {
                Code = catalog.Code,
                Name = string.IsNullOrWhiteSpace(catalog.Name) ? catalog.Code : catalog.Name,
                Icons = [.. icons.Where(i => i.Catalog == catalog.Code).Select(i =>
                {
                    return new IconResult
                    {
                        Catalog = i.Catalog,
                        Path = i.Path,
                        Url = i.Host + i.Path,
                    };
                })],
            });
        }

        return Result.Success(result);
    }
}
