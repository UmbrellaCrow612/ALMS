﻿namespace ALMS.API.DTOs.Users
{
    public class UserDto
    {
        public required string Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Address { get; set; }
        public required string Email { get; set; }
        public required string UserName { get; set; }
        public required string PhoneNumber { get; set; }
    }
}
