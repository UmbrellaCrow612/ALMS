using ALMS.API.Controllers;
using ALMS.API.Data;
using ALMS.API.Data.Models;
using ALMS.API.DTOs.Users;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AMLS.API.Test
{
    public class ManageProfile_Test
    {
        private readonly ProfileController _controller;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;


        public ManageProfile_Test()
        {
            // Set up in-memory database for ApplicationDbContext
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            _dbContext = new ApplicationDbContext(options);

            

            // Set up AutoMapper
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UpdateUserDto, ApplicationUser>()
                    .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                    .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                    .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                    .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                    .ForMember(dest => dest.IsApproved, opt => opt.Ignore())
                    .ForMember(dest => dest.MembershipStatus, opt => opt.Ignore())
                    .ForMember(dest => dest.MaxBorrowLimit, opt => opt.Ignore())
                    .ForMember(dest => dest.BorrowedItems, opt => opt.Ignore())
                    .ForMember(dest => dest.ForgotPasswordAttempts, opt => opt.Ignore())
                    .ForMember(dest => dest.Subscriptions, opt => opt.Ignore())
                    .ForMember(dest => dest.YSubscriptions, opt => opt.Ignore())
                    .ForMember(dest => dest.Id, opt => opt.Ignore())
                    .ForMember(dest => dest.UserName, opt => opt.Ignore())
                    .ForMember(dest => dest.NormalizedUserName, opt => opt.Ignore())
                    .ForMember(dest => dest.Email, opt => opt.Ignore())
                    .ForMember(dest => dest.NormalizedEmail, opt => opt.Ignore())
                    .ForMember(dest => dest.EmailConfirmed, opt => opt.Ignore())
                    .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                    .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
                    .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore())
                    .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.Ignore())
                    .ForMember(dest => dest.TwoFactorEnabled, opt => opt.Ignore())
                    .ForMember(dest => dest.LockoutEnd, opt => opt.Ignore())
                    .ForMember(dest => dest.LockoutEnabled, opt => opt.Ignore())
                    .ForMember(dest => dest.StripeSessions, opt => opt.Ignore())    
                    .ForMember(dest => dest.AccessFailedCount, opt => opt.Ignore());

                //mapping for ApplicationUser to UserDto
                  cfg.CreateMap<ApplicationUser, UserDto>()
                     .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                     .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                     .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                     .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                     .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                     .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName));
    
            });

            // Validate the AutoMapper configuration
            mapperConfig.AssertConfigurationIsValid();

            // Initialize the AutoMapper instance
            _mapper = mapperConfig.CreateMapper();
            _controller = new ProfileController(_dbContext, _mapper);
        }

        [Fact]
        public async Task UpdateProfile_ShouldUpdateUser_WhenValidDataIsProvided()
        {
            // Add an initial user to the in-memory database
            _dbContext.Users.Add(new ApplicationUser
            {
                Id = "123",
                FirstName = "cLAIR",
                LastName = "Doe",
                Address = "Old Address",
                PhoneNumber = "987654321",
                MaxBorrowLimit = 10,
                MembershipStatus = MembershipStatus.Active
            });
            _dbContext.SaveChanges();
            // Arrange
            var memberId = "123";  // The user to update
            var updatedProfileData = new UpdateUserDto
            {
                FirstName = "Updated",
                LastName = "User",
                Address = "New Address",
                PhoneNumber = "123456789"
            };

            // Act
            var result = await _controller.UpdateUser(updatedProfileData, memberId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);  // Should return Ok result
            Assert.Equal("Profile updated successfully.", okResult.Value);  // Verify success message

            // Verify the profile is updated in the in-memory database
            var updatedUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == memberId);
            Assert.Equal("New Address", updatedUser.Address);  // Check updated address
            Assert.Equal("123456789", updatedUser.PhoneNumber);  // Check updated phone number
            Assert.Equal(10, updatedUser.MaxBorrowLimit);  // Ensure MaxBorrowLimit remains the same
        }

        [Fact]
        public async Task UpdateProfile_ShouldReturnNotFound_WhenProfileDoesNotExist()
        {
            // Add an initial user to the in-memory database
            _dbContext.Users.Add(new ApplicationUser
            {
                Id = "1235",
                FirstName = "UAGDWW",
                LastName = "JDKABWD",
                Address = "Old Address",
                PhoneNumber = "987654321",
                MaxBorrowLimit = 10,
                MembershipStatus = MembershipStatus.Active
            });
            _dbContext.SaveChanges();
            // Arrange
            var memberId = "999"; // Non-existent member
            var updatedProfileData = new UpdateUserDto
            {
                FirstName = "AGYDYA",
                LastName = "JKAWD",
                Address = "New Address",
                PhoneNumber = "123456789"
            };

            // Act
            var result = await _controller.UpdateUser(updatedProfileData, memberId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task UpdateProfile_ShouldReturnBadRequest_WhenInvalidDataIsProvided()
        {
            // Add an initial user to the in-memory database
            _dbContext.Users.Add(new ApplicationUser
            {
                Id = "124",
                FirstName = "DUAOWD",
                LastName = "DKLAWD",
                Address = "KALADNA",
                PhoneNumber = "987654321",
                MaxBorrowLimit = 10,
                MembershipStatus = MembershipStatus.Active
            });
            _dbContext.SaveChanges();
            // Arrange
            var memberId = "124";
            var updatedProfileData = new UpdateUserDto
            {
                FirstName = "Updated",  // Missing last name and address
                LastName = "",
                Address = "",
                PhoneNumber = ""
            };

            // Act
            var result = await _controller.UpdateUser(updatedProfileData, memberId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid data entered. Please correct and try again.", badRequestResult.Value);
        }
        [Fact]
        public async Task GetProfile_ShouldReturnFilteredResults_WhenQueryMatches()
        {
            // Arrange: Add sample users to the in-memory database
            _dbContext.Users.AddRange(new List<ApplicationUser>
            {
                 new ApplicationUser { Id = "11342", FirstName = "John", LastName = "Smith", Email = "john.smith@example.com", PhoneNumber = "123456789", Address = "123 Elm Street", UserName = "johnsmith", MaxBorrowLimit = 10, MembershipStatus = MembershipStatus.Active },
                 new ApplicationUser { Id = "22234", FirstName = "AIOHWDA", LastName = "ODAHWDI", Email = "jane.doe@example.com", PhoneNumber = "987654321", Address = "456 Oak Street", UserName = "janedoe" , MaxBorrowLimit = 10, MembershipStatus = MembershipStatus.Active }
            });
            _dbContext.SaveChanges();

            var query = new SearchUserQuery
            {
                FirstName = "John",
                LastName = "Smith"
            };

            // Act
            var result = await _controller.GetProfile(query);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result); // Extract Result from ActionResult
            var returnedUsers = Assert.IsType<List<UserDto>>(okResult.Value);

            Assert.Single(returnedUsers);
            Assert.Contains(returnedUsers, u => u.FirstName == "John" && u.LastName == "Smith");
        }

        [Fact]
        public async Task GetProfile_ShouldReturnEmptyList_WhenNoMatchFound()
        {
            // Arrange: Add sample users to the in-memory database
            _dbContext.Users.AddRange(new List<ApplicationUser>
             {
                 new ApplicationUser { Id = "111", FirstName = "DALHWKD", LastName = "AGDWYIUHAUDW", Email = "john.smith@example.com", PhoneNumber = "123456789", Address = "123 Elm Street", UserName = "johnsmith", MaxBorrowLimit = 10, MembershipStatus = MembershipStatus.Active},
                 new ApplicationUser { Id = "222", FirstName = "DKNALUKWD", LastName = "ADHAUKWD", Email = "jane.doe@example.com", PhoneNumber = "987654321", Address = "456 Oak Street", UserName = "janedoe" ,MaxBorrowLimit = 10, MembershipStatus = MembershipStatus.Active }
             });
            _dbContext.SaveChanges();

            var query = new SearchUserQuery
            {
                FirstName = "Nonexistent"
            };

            // Act
            var result = await _controller.GetProfile(query);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUsers = Assert.IsType<List<UserDto>>(okResult.Value);

            Assert.Empty(returnedUsers);
        }
        [Fact]
        public async Task GetProfile_ShouldReturnPartialMatches_WhenQueryPartiallyMatches()
        {
            // Arrange: Add sample users to the in-memory database
            _dbContext.Users.AddRange(new List<ApplicationUser>
              {
                 new ApplicationUser { Id = "812", FirstName = "Tommy", LastName = "dioauiwd", Email = "johnny.smith@example.com", PhoneNumber = "123456789", Address = "123 Elm Street", UserName = "johnnysmith", MaxBorrowLimit = 10, MembershipStatus = MembershipStatus.Active },
                new ApplicationUser { Id = "922", FirstName = "dakwbd", LastName = "ayuwdh", Email = "jane.doe@example.com", PhoneNumber = "987654321", Address = "456 Oak Street", UserName = "janedoe" , MaxBorrowLimit = 10, MembershipStatus = MembershipStatus.Active }
            });
            _dbContext.SaveChanges();

            var query = new SearchUserQuery
            {
                FirstName = "Tom"
            };

            // Act
            var result = await _controller.GetProfile(query);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUsers = Assert.IsType<List<UserDto>>(okResult.Value);

            Assert.Single(returnedUsers);
            Assert.Contains(returnedUsers, u => u.FirstName == "Tommy");
        }

        [Fact]
        public async Task GetProfile_ShouldReturnAllUsers_WhenNoQueryParametersProvided()
        {
            // Arrange: Add sample users to the in-memory database
            _dbContext.Users.AddRange(new List<ApplicationUser>
            {
                    new ApplicationUser { Id = "317", FirstName = "uiagiohdw", LastName = "dlmkahukgwdkl", Email = "john.smith@example.com", PhoneNumber = "123456789", Address = "123 Elm Street", UserName = "johnsmith",MaxBorrowLimit = 10, MembershipStatus = MembershipStatus.Active },
                    new ApplicationUser { Id = "425", FirstName = "dljakjwdkgla", LastName = "dknlakwhvd", Email = "jane.doe@example.com", PhoneNumber = "987654321", Address = "456 Oak Street", UserName = "janedoe",MaxBorrowLimit = 10, MembershipStatus = MembershipStatus.Active }
            });
            _dbContext.SaveChanges();

            var query = new SearchUserQuery();

            // Act
            var result = await _controller.GetProfile(query);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUsers = Assert.IsType<List<UserDto>>(okResult.Value);

            Assert.Equal(8, returnedUsers.Count);
        }
    }
}

