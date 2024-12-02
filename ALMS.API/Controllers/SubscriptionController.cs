using ALMS.API.Core.Constants;
using ALMS.API.Data;
using ALMS.API.Data.Models;
using ALMS.API.DTOs.Media;
using ALMS.API.DTOs.Subscription;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;
using System.Globalization;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using Subscription = ALMS.API.Data.Models.Subscription;

namespace ALMS.API.Controllers
{
    [ApiController]
    [Route("subscription")]
    public class SubscriptionController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IMapper mapper) : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        [Authorize]
        [HttpPost("subscribe/{stripeProductId}")]
        public async Task<ActionResult> Subscribe(string stripeProductId)
        {

            var lineItems = new List<SessionLineItemOptions>();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            StripeProductEntity theProduct = _dbContext.StripeProducts.FirstOrDefault(p => p.Id == stripeProductId);
            if (theProduct is null)
            {
                return NotFound("Stripe Product Not Found");
            }
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                LineItems = lineItems,
                Mode = "payment",
                // Update to use localhost URLs for testing
                SuccessUrl = "http://localhost:5173/",
                CancelUrl = "http://localhost:5173/"
            };

            var products = await _dbContext.StripeProducts.ToListAsync();

            // Loop through products to add them to the session

            decimal price = decimal.TryParse(theProduct.Rate.Replace("£", ""), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result) ? result : 0m;
            var sessionListItem = new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(price * 100), // Stripe expects the amount in cents
                    Currency = "GBP",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = theProduct.Product.ToString(),
                    }
                },
                Quantity = theProduct.Quanity
            };
            options.LineItems.Add(sessionListItem);



            var service = new Stripe.Checkout.SessionService();
            Stripe.Checkout.Session session = service.Create(options);

            // Ensure session URL is valid
            if (string.IsNullOrEmpty(session.Url))
            {
                return BadRequest("Stripe session creation failed. No URL returned.");
            }

            await _dbContext.StripeSessions.AddAsync(new StripeSession { SessionId = session.Id, SessionUrl = session.Url, UserId = userId, ProductId = theProduct.Id });
            await _dbContext.SaveChangesAsync();

            var redirectUrl = session.Url;
            return Ok(redirectUrl);
        }

        [Authorize]
        [HttpGet("GetSubscription/{userId}")]
        public async Task<ActionResult> GetSub(string userId)
        {
            if (userId == "user1")
            {
                List<Subscription> testSubscriptions = await _dbContext.Subscriptions.Where(x => x.UserId == userId).ToListAsync();
                return Ok(testSubscriptions);
            }

            var stripeSessions = await _dbContext.StripeSessions.ToListAsync();

            foreach (var stripeSession in stripeSessions)
            {
                var sessionId = stripeSession.SessionId;
                var service = new Stripe.Checkout.SessionService();
                var session = service.Get(sessionId);



                var subExisted = await _dbContext.Subscriptions.AnyAsync(s => s.UserId == stripeSession.UserId && s.ProductId == stripeSession.ProductId);

                if (!subExisted)
                {
                    Subscription sub;
                    // Successful payment, create the subscription

                    sub = new Subscription
                    {
                        Status = session.Status,
                        UserId = stripeSession.UserId,
                        StartDate = session.Created,
                        EndDate = session.Created.AddMonths(1),
                        ProductId = stripeSession.ProductId
                    };
                    await _dbContext.Subscriptions.AddAsync(sub);
                    await _dbContext.SaveChangesAsync();
                }
                
            }

            List<Subscription> subscriptions = await _dbContext.Subscriptions.Where(x => x.UserId == userId).ToListAsync();

            if (subscriptions.Count() == 0)
            {
                return BadRequest("No session ID found.");
            }
            else
            {
                return Ok(subscriptions);
            }
        }

        [Authorize(Roles = UserRoles.Accountant)]
        [HttpPatch("update/{subscriptionId}")]
        public async Task<ActionResult> UpdateSub([FromBody] UpdateSubscriptionDto updateSubscriptionDto, string subscriptionId)
        {

            var subscriptonToUpdate = await _dbContext.Subscriptions.FirstOrDefaultAsync(x => x.Id == subscriptionId);

            if (subscriptonToUpdate is null) return NotFound();

            _mapper.Map(updateSubscriptionDto, subscriptonToUpdate);

            await _dbContext.SaveChangesAsync();

            return Ok(subscriptonToUpdate);
        }

        [Authorize]
        [HttpGet("stripeProducts")]
        public async Task<ActionResult> GetStripeProducts()
        {
            var products = await _dbContext.StripeProducts.ToListAsync();

            return Ok(products);
        }
    }
}