using Ecom.BLL.ModelVM.FaceId;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FaceIdController : ControllerBase
    {
        private readonly IFaceIdService _faceService;

        public FaceIdController(IFaceIdService faceService)
        {
            _faceService = faceService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterFaceIdVM model)
        {
            var result = await _faceService.RegisterFaceAsync(model);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] UpdateFaceIdVM model)
        {
            var result = await _faceService.UpdateFaceAsync(model);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPut("delete")]
        public async Task<IActionResult> Delete(string userId)
        {
            var result = await _faceService.DeleteFaceAsync(userId);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("verify/{userId}")]
        public async Task<IActionResult> Verify(string userId)
        {
            var result = await _faceService.VerifyFaceByUserIdAsync(userId);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpPost("login")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Login(IFormFile image)
        {
            var result = await _faceService.VerifyFaceLoginAsync(image);
            return result.IsSuccess ? Ok(result) : Unauthorized(result);
        }
    }
}
