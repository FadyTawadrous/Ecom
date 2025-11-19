
namespace Ecom.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        /// <summary>
        /// A helper property to get the currently logged-in user's ID
        /// from their JWT token. This will be null if the user is not authenticated.
        /// </summary>
        protected string? CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
