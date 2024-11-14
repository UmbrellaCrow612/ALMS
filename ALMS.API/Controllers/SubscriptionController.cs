using ALMS.API.Core.Constants;
using ALMS.API.Data;
using ALMS.API.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;
using Subscription = ALMS.API.Data.Models.Subscription;

namespace ALMS.API.Controllers
{
    [ApiController]
    [Route("subscription")]
    public class SubscriptionController(ApplicationDbContext dbContext) : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        [Authorize]
        [HttpPost("subscribe")]
        public async Task<ActionResult> Subscribe()
        {

            var lineItems = new List<SessionLineItemOptions>();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var options = new Stripe.Checkout.SessionCreateOptions
            {
                LineItems = lineItems,
                Mode = "payment",
                // Update to use localhost URLs for testing
                SuccessUrl = "https://localhost:7066/swagger/index.html",
                CancelUrl = "https://localhost:7066/swagger/index.html"
            };

            var products = await _dbContext.StripeProducts.ToListAsync();

            // Loop through products to add them to the session
            foreach (var product in products)
            {
                var sessionListItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(4.99 * 100), // Stripe expects the amount in cents
                        Currency = "GBP",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = product.Product.ToString(),
                        }
                    },
                    Quantity = product.Quanity
                };
                options.LineItems.Add(sessionListItem);
            }

            var service = new Stripe.Checkout.SessionService();
            Stripe.Checkout.Session session = service.Create(options);

            // Ensure session URL is valid
            if (string.IsNullOrEmpty(session.Url))
            {
                return BadRequest("Stripe session creation failed. No URL returned.");
            }

            await _dbContext.StripeSessions.AddAsync(new StripeSession { SessionId = session.Id, SessionUrl = session.Url, UserId = userId });
            await _dbContext.SaveChangesAsync();

            var redirectUrl = session.Url;
            return Ok(redirectUrl);
        }

        [Authorize]
        [HttpGet("subscribersDetails")]
        public async Task<ActionResult> GetSubscribersDetails()
        {
            List<Subscription> subscriptions = [];
            foreach (var stripeSession in _dbContext.StripeSessions)
            {
                var sessionId = stripeSession.SessionId;
                var service = new Stripe.Checkout.SessionService();
                var session = service.Get(sessionId);

                Subscription sub;
                // Successful payment, create the subscription
                sub = new Subscription
                {
                    Status = session.PaymentStatus.ToString(),
                    UserId = stripeSession.UserId,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(1)
                };
                await _dbContext.Subscriptions.AddAsync(sub);
                await _dbContext.SaveChangesAsync();
                subscriptions.Add(sub);
            }
            if (subscriptions.Count() == 0)
            {
                return BadRequest("No session ID found.");
            }
            else
            {
                return Ok(subscriptions);
            }
        }
    }
}