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
                    .ForMember(dest => dest.StartDate, opt => opt.Ignore());

                // Mapping for CreateStripeProductDto to StripeProductEntity (for product creation)
                cfg.CreateMap<CreateStripeProductDto, StripeProductEntity>()
                   .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
                    .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.Rate))
                    .ForMember(dest => dest.Quanity, opt => opt.MapFrom(src => src.Quanity))
                    .ForMember(dest => dest.Id, opt => opt.Ignore());


                // Mapping for UpdateStripeProductDto to StripeProductEntity (for product updates)
                cfg.CreateMap<UpdateStripeProductDto, StripeProductEntity>()
                    .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
                    .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.Rate))
                    .ForMember(dest => dest.Quanity, opt => opt.MapFrom(src => src.Quanity))
                    .ForMember(dest => dest.Id, opt => opt.Ignore()); // Assuming Id is auto-generated
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

        //[Fact]
        //public async Task GetSubscribersDetails_ShouldReturnOk_WithValidSessions()
        //{
        //    // Arrange: Add mock Stripe session to the in-memory database
        //    var stripeSession = new StripeSession
        //    {
        //        SessionId = "sessionGetSubscribersDetails",
        //        SessionUrl = "http://example.com",
        //        UserId = "user1"
        //    };
        //    _dbContext.StripeSessions.Add(stripeSession);
        //    await _dbContext.SaveChangesAsync();

        //    // Mock the response from the Stripe API for the session
        //    var mockStripeSession = new Stripe.Checkout.Session
        //    {
        //        Id = "sessionGetSubscribersDetails",
        //        PaymentStatus = "paid",
        //        Url = "http://example.com"
        //    };

        //    // Mock the Stripe SessionService.Get method to return the mock session when called with the correct session ID
        //    _mockSessionService.Setup(service =>
        //        service.Get("sessionGetSubscribersDetails", It.IsAny<SessionGetOptions>(), It.IsAny<RequestOptions>()))
        //        .Returns(mockStripeSession);

        //    // Act: Call the GetSubscribersDetails method in the controller
        //    var result = await _controller.GetSubscribersDetails();

        //    // Assert: Check if the result is Ok and the list of subscriptions is returned
        //    var actionResult = Assert.IsType<OkObjectResult>(result);
        //    var subscriptions = Assert.IsType<List<ALMS.API.Data.Models.Subscription>>(actionResult.Value);
        //    Assert.Single(subscriptions);  // Ensure only one subscription is returned
        //    Assert.Equal("user1", subscriptions[0].UserId);  // Verify the userId
        //}
    }
}
