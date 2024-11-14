using ALMS.API.Core.Constants;
using ALMS.API.Data;
using ALMS.API.Data.Models;
using ALMS.API.DTOs.Media;
using ALMS.API.DTOs.Subscription;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Stripe;
using Stripe.Checkout;
using System.Globalization;
using System.Security.Claims;
using Subscription = ALMS.API.Data.Models.Subscription;

namespace ALMS.API.Controllers
{
    [ApiController]
    [Route("subscription")]
    public class SubscriptionController(ApplicationDbContext dbContext, IMapper mapper) : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;

        [Authorize]
        [HttpPost("subscribe{id}")]
        public async Task<ActionResult> Subscribe(string id)
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

            // Loop through products to add them to the session
            foreach (var product in _dbContext.StripeProducts)
            {
                if (product.Id == id)
                {
                    decimal price = decimal.TryParse(product.Rate.Replace("£", ""), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result) ? result : 0m;
                    var sessionListItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(price * 100), // Stripe expects the amount in cents
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

        [Authorize(Roles = UserRoles.Accountant)]
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

        [Authorize]
        [HttpGet("mysubscription")]
        public async Task<ActionResult> GetSub()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var subscriptionDetails = await _dbContext.Subscriptions.Where(x => x.UserId == userId).ToListAsync();

            return Ok(subscriptionDetails);
        }

        [Authorize(Roles = UserRoles.Accountant)]
        [HttpPatch("update/{id}")]
        public async Task<ActionResult> UpdateSub([FromBody] UpdateSubscriptionDto updateSubscriptionDto, string id)
        {

            var subscriptonToUpdate = await _dbContext.Subscriptions.FirstOrDefaultAsync(x => x.Id == id);

            if (subscriptonToUpdate is null) return NotFound();

            _mapper.Map(updateSubscriptionDto, subscriptonToUpdate);

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = UserRoles.Accountant)]
        [HttpPost("stripeSubscription/create")]
        public async Task<ActionResult> CreateStripeProduct([FromBody] CreateStripeProductDto createStripeProductDto)
        {

            var stripeProduct = _mapper.Map<StripeProductEntity>(createStripeProductDto);

            await _dbContext.StripeProducts.AddAsync(stripeProduct);
            await _dbContext.SaveChangesAsync();

            return Created(nameof(createStripeProductDto), new { stripeProduct.Id });
        }

        [Authorize(Roles = UserRoles.Accountant)]
        [HttpPatch("stripeSubscription/update/{id}")]
        public async Task<ActionResult> UpdateStripeProduct([FromBody] UpdateStripeProductDto updateStripeProductDto, string id)
        {

            var StripeProductToUpdate = await _dbContext.StripeProducts.FirstOrDefaultAsync(x => x.Id == id);

            if (StripeProductToUpdate is null) return NotFound();

            _mapper.Map(updateStripeProductDto, StripeProductToUpdate);

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = UserRoles.Accountant)]
        [HttpDelete("stripeSubscription/delete/{id}")]
        public async Task<ActionResult> DeleteStripeSubscriptionItem(string id)
        {
            var stripeSubscriptionToDelete = await _dbContext.StripeProducts.FirstOrDefaultAsync(x => x.Id == id);
            if (stripeSubscriptionToDelete is null) return NotFound();

            _dbContext.StripeProducts.Remove(stripeSubscriptionToDelete);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}