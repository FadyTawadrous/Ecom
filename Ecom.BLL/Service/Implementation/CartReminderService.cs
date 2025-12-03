
using Ecom.DAL.Database;

namespace Ecom.BLL.Service.Implementation
{
    public class CartReminderService : ICartReminderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public CartReminderService(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task SendAbandonedCartEmailsAsync()
        {
            // 1. Define "Abandoned": Inactive for 2 days (48 hours)
            var cutOffTime = DateTime.UtcNow.AddDays(-2);

            // 2. Query Carts
            var abandonedCarts = await _context.Carts
                .Include(c => c.CartItems) // Need items to see what they forgot
                .Include(c => c.AppUser)   // Need User to get Email address
                .Where(c =>
                    !c.IsDeleted && // Cart exists
                    c.CartItems!.Any(i => !i.IsDeleted) && // Cart is not empty
                                                           // Check activity date (Use UpdatedOn if available, else CreatedOn)
                    (c.UpdatedOn.HasValue ? c.UpdatedOn < cutOffTime : c.CreatedOn < cutOffTime)
                )
                .ToListAsync();

            if (!abandonedCarts.Any()) return;

            foreach (var cart in abandonedCarts)
            {
                // 3. Prepare Email Content
                var userEmail = cart.AppUser.Email;
                var itemCount = cart.CartItems!.Count;
                var subject = "You left something behind! 🛒";

                // You can make this HTML much prettier later
                var message = $@"
                    <h3>Hi {cart.AppUser.DisplayName},</h3>
                    <p>We noticed you left <strong>{itemCount} items</strong> in your cart.</p>
                    <p>They are selling out fast! Click below to complete your order:</p>
                    <a href='http://localhost:4200/cart'>Return to Checkout</a>
                ";

                // 4. Send Email
                // Note: We await inside the loop here. For thousands of emails, 
                // you might want to batch this or queue individual email jobs.
                bool emailSent = await _emailService.SendEmailAsync(userEmail!, subject, message);

            }

            // 6. Save all changes (updates the flags)
            await _context.SaveChangesAsync();
        }

    }
}
