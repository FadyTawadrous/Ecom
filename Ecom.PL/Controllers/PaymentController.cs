
using Ecom.BLL.ModelVM.Payment;
using Ecom.DAL.Entity;

namespace Ecom.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : BaseApiController
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        /// <summary>
        /// Initiates a payment for an existing Order.
        /// </summary>
        [HttpPost("create")]
        [Authorize] // Must be logged in
        public async Task<ActionResult<Payment>> CreatePayment([FromBody] CreatePaymentVM model)
        {
            if (CurrentUserId == null)
            {
                return Unauthorized();
            }

            var result = await _paymentService.CreatePaymentRecordAsync(model, CurrentUserId);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.ErrorMessage });
            }

            return Ok(result.Result);
        }

        /// <summary>
        /// Simulates a Webhook from a Payment Gateway (e.g., Stripe).
        /// In a real app, this endpoint would be public but verified with a specific Header Signature.
        /// </summary>
        [HttpPost("webhook")]
        [AllowAnonymous] // Webhooks come from external servers, not logged-in users
        public async Task<ActionResult<bool>> UpdatePaymentStatus([FromBody] PaymentResultVM model)
        {
            // In a real scenario, 'UpdatedBy' would be "System" or "StripeWebhook"
            var result = await _paymentService.UpdatePaymentStatusAsync(model, "SystemWebhook");

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.ErrorMessage });
            }

            return Ok(new { message = "Payment status updated successfully" });
        }

        /// <summary>
        /// Get Payment details by Order ID.
        /// </summary>
        [HttpGet("payment/{orderId}")]
        [Authorize]
        public async Task<ActionResult<Payment>> GetPaymentByOrder(int orderId)
        {
            var result = await _paymentService.GetPaymentByOrderIdAsync(orderId);

            if (!result.IsSuccess)
            {
                return NotFound(new { message = result.ErrorMessage });
            }

            // Optional: Check if the payment belongs to the current user 
            // (You would need to map Order inside Payment to check AppUserId)

            return Ok(result.Result);
        }

        // --- Admin Endpoints ---

        [HttpGet("payments")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<GetPaymentVM>>> GetAllPayments()
        {
            var result = await _paymentService.GetAllPaymentsAsync();
            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.ErrorMessage });
            }
            return Ok(result.Result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> ToggleDelete(int id)
        {
            if (CurrentUserId == null) return Unauthorized();

            var result = await _paymentService.ToggleDeleteStatusAsync(id, CurrentUserId);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.ErrorMessage });
            }

            return Ok(new { message = "Payment delete status toggled" });
        }

    }
}
