
namespace Ecom.BLL.Responses
{
    public record ResponseResult<T>(T? Result, string? ErrorMessage, bool IsSuccess);
}
