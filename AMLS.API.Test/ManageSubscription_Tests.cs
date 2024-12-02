using ALMS.API.Controllers;
using ALMS.API.Data.Models;
using ALMS.API.Data;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ALMS.API.DTOs.Subscription;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace AMLS.API.Test
{
    public class ManageSubscription_Tests
    {
        private readonly SubscriptionController _controller;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<SessionService> _mockSessionService;
        private readonly Mock<ClaimsPrincipal> _mockClaimsPrincipal;

        public ManageSubscription_Tests()
        {

            Stripe.StripeConfiguration.ApiKey = "sk_test_4eC39HqLyjWDarjtT1zdp7dc"; // Dummy key for testing

            // Set up in-memory database for ApplicationDbContext
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            _dbContext = new ApplicationDbContext(options);

            // Set up AutoMapper
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                // Mapping for Subscription to SubscriptionDto
                cfg.CreateMap<ALMS.API.Data.Models.Subscription, SubscriptionDto>()
                    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                    .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                    .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                    .ForMember(dest => dest.Id, opt => opt.Ignore())
                    .ForMember(dest => dest.NameType, opt => opt.Ignore());


                // Mapping for UpdateSubscriptionDto to Subscription (for updates)
                cfg.CreateMap<UpdateSubscriptionDto, ALMS.API.Data.Models.Subscription>()
                    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                    .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                    .ForMember(dest => dest.Id, opt => opt.Ignore())
                    .ForMember(dest => dest.User, opt => opt.Ignore())
                    .ForMember(dest => dest.UserId, opt => opt.Ignore())
                    .ForMember(dest => dest.StripeProduct, opt => opt.Ignore())
                    .ForMember(dest => dest.ProductId, opt => opt.Ignore())
                    .ForMember(dest => dest.StartDate, opt => opt.Ignore());
            });

            mapperConfig.AssertConfigurationIsValid();
            _mapper = mapperConfig.CreateMapper();


            var mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
            mockClaimsPrincipal.Setup(u => u.FindFirst(ClaimTypes.NameIdentifier)).Returns(new Claim(ClaimTypes.NameIdentifier, "user1"));

            // Mocking UserManager
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(),
                null, null, null, null, null, null, null, null);
            // Mocking Stripe.SessionService

            _mockSessionService = new Mock<SessionService>();
            _controller = new SubscriptionController(_dbContext, _mockUserManager.Object, _mapper)
            {
                // Mocking the User property directly on the controller
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext() { User = mockClaimsPrincipal.Object }
                }
            };

        }

        [Fact]
        public async Task Subscribe_ShouldReturnOk_WithValidProductId()
        {

            // Arrange: Add a valid Stripe product to the in-memory database
            var product = new StripeProductEntity
            {
                Id = "validProductId",
                Product = "Test Product",
                Rate = "£10.00",
                Quanity = 1
            };
            _dbContext.StripeProducts.Add(product);
            await _dbContext.SaveChangesAsync();

            // Mock UserManager to return a user ID
            _mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser
            {
                Id = "user1",
                FirstName = "John",
                LastName = "Doe",
                Address = "123 Main St",
                MembershipStatus = MembershipStatus.Active,
                MaxBorrowLimit = 5
            });

            // Mock SessionService to simulate Stripe session creation
            var mockSession = new Stripe.Checkout.Session
            {
                Id = "session123",
                Url = "http://example.com"
            };

            // Mock the creation of the session with explicit arguments and ignore the optional `requestOptions`
            _mockSessionService.Setup(service =>
                service.Create(It.Is<SessionCreateOptions>(o =>
                    o.Mode == "payment" &&
                    o.SuccessUrl == "http://localhost:5173/" &&
                    o.CancelUrl == "http://localhost:5173/" &&
                    o.LineItems.Count == 1
                ), It.IsAny<RequestOptions>()))  // Ignore optional `requestOptions`
                .Returns(mockSession);

            var stripeProductId = "validProductId";

            // Act: Call the Subscribe method in the controller
            var result = await _controller.Subscribe(stripeProductId);

            // Assert: Check if the result is Ok
            var actionResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(actionResult.Value);
            Assert.Contains("https://checkout.stripe.com/", actionResult.Value.ToString());
        }

        [Fact]
        public async Task Subscribe_ShouldReturnNotFound_WhenInvalidProductId()
        { 
            
            // Arrange: Add a valid Stripe product to the in-memory database
            var product = new StripeProductEntity
            {
                Id = "validProductId2",
                Product = "Test Product",
                Rate = "£10.00",
                Quanity = 1
            };
            _dbContext.StripeProducts.Add(product);
            await _dbContext.SaveChangesAsync();
            // Arrange
            var stripeProductId = "invalidProductId";

            var mockSession = new Stripe.Checkout.Session
            {
                Id = "session123",
                Url = "http://example.com"
            };
            // Mock the creation of the session with explicit arguments and ignore the optional `requestOptions`
            _mockSessionService.Setup(service =>
                service.Create(It.Is<SessionCreateOptions>(o =>
                    o.Mode == "payment" &&
                    o.SuccessUrl == "http://localhost:5173/" &&
                    o.CancelUrl == "http://localhost:5173/" &&
                    o.LineItems.Count == 1
                ), It.IsAny<RequestOptions>()))  // Ignore optional `requestOptions`
                .Returns(mockSession);
            // Act
            var result = await _controller.Subscribe(stripeProductId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Stripe Product Not Found", notFoundResult.Value);
        }
        [Fact]
        public async Task GetSub_ShouldReturnOk_WithValidUserId()
        {
            // Arrange: Add mock Stripe session to the in-memory database
            var session = new StripeSession
            {
                SessionId = "cs_test_a12PCVw51ozVrz5JnG2tTISqtmqmH7xopYK991agv1kGuEWhQwHTsvpWNU",
                SessionUrl = "http://example.com",
                UserId = "user1",
                ProductId = "product1"
            };
            _dbContext.StripeSessions.Add(session);
            await _dbContext.SaveChangesAsync();

            var mockSessionService = new Mock<Stripe.Checkout.SessionService>();
            var mockSession = new Session
            {
                Id = "cs_test_a12PCVw51ozVrz5JnG2tTISqtmqmH7xopYK991agv1kGuEWhQwHTsvpWNU",
                Created = DateTime.Now// Example Unix timestamp (January 1, 2021)
            };

            // Mocking the Get method to return the mock session
            var service = mockSessionService.Object;
            mockSessionService.Setup(service => service.Get("cs_test_a12PCVw51ozVrz5JnG2tTISqtmqmH7xopYK991agv1kGuEWhQwHTsvpWNU", It.IsAny<SessionGetOptions>(), It.IsAny<RequestOptions>()))
                              .Returns(mockSession);

            var sub = new ALMS.API.Data.Models.Subscription
            {
                Status = "paid",
                EndDate= DateTime.UtcNow.AddMonths(1),
                StartDate = DateTime.UtcNow,
                UserId = "user1",
                ProductId = "product1"
            };
            _dbContext.Subscriptions.Add(sub);
            await _dbContext.SaveChangesAsync();

            // Act: Call GetSub method in the controller
            var result = await _controller.GetSub("user1");

            // Assert: Check if the result is Ok and contains subscription data
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var subscriptions = Assert.IsType<List<ALMS.API.Data.Models.Subscription>>(actionResult.Value);
            Assert.Single(subscriptions);  // Ensure only one subscription is returned
            Assert.Equal("user1", subscriptions[0].UserId);  // Verify the userId
        }

        [Fact]
        public async Task GetSub_ShouldReturnBadRequest_WhenNoSubscriptionsExist()
        {
            // Arrange: Ensure there are no subscriptions in the database for the user
            var result = await _controller.GetSub("user2");

            // Assert: Ensure the result is a BadRequest
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("No session ID found.", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateSub_ShouldReturnOk_WithValidData()
        {
            // Arrange: Add a subscription to the in-memory database
            var subscription = new ALMS.API.Data.Models.Subscription
            {
                Id = "sub123",
                Status = "active",
                UserId = "user1",
                ProductId = "product1",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(1)
            };
            _dbContext.Subscriptions.Add(subscription);
            await _dbContext.SaveChangesAsync();

            var updateSubscriptionDto = new UpdateSubscriptionDto
            {
                Status = "inactive",
                EndDate = DateTime.UtcNow.AddMonths(2)
            };

            // Act: Call UpdateSub method in the controller
            var result = await _controller.UpdateSub(updateSubscriptionDto, "sub123");

            // Assert: Check if the result is Ok and subscription data is updated
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var updatedSubscription = Assert.IsType<ALMS.API.Data.Models.Subscription>(actionResult.Value);
            Assert.Equal("inactive", updatedSubscription.Status);
            Assert.Equal(DateTime.UtcNow.AddMonths(2).ToString(), updatedSubscription.EndDate.ToString());
        }

        [Fact]
        public async Task UpdateSub_ShouldReturnNotFound_WhenInvalidSubscriptionId()
        {
            // Arrange: Ensure there are no subscriptions in the in-memory database
            var updateSubscriptionDto = new UpdateSubscriptionDto
            {
                Status = "inactive",
                EndDate = DateTime.UtcNow.AddMonths(2)
            };

            // Act: Call UpdateSub method in the controller with an invalid subscriptionId
            var result = await _controller.UpdateSub(updateSubscriptionDto, "invalidSubId");

            // Assert: Ensure the result is NotFound
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetStripeProducts_ShouldReturnOk_WithProducts()
        {
            // Arrange: Add some Stripe products to the in-memory database
            var product = new StripeProductEntity
            {
                Id = "product1",
                Product = "Test Product",
                Rate = "£10.00",
                Quanity = 1
            };
            _dbContext.StripeProducts.Add(product);
            await _dbContext.SaveChangesAsync();

            // Act: Call GetStripeProducts method in the controller
            var result = await _controller.GetStripeProducts();

            // Assert: Ensure the result is Ok and contains product data
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var products = Assert.IsType<List<StripeProductEntity>>(actionResult.Value);
            Assert.Contains(products, p => p.Product == "Test Product");
        }
    }
}
