
namespace Ecom.BLL.Responses
{
    public record PaginatedResult<T>(IEnumerable<T> Items, int TotalCount,
        int PageNumber, int PageSize);
}
