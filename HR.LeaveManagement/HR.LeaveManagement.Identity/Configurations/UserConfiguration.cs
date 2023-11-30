using HR.LeaveManagement.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Identity.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();
            builder.HasData(
                new ApplicationUser
                {
                    Id = "e03da57b-54f0-4276-98b9-95d08a6d4749",
                    Email = "admin@gmail.com",
                    NormalizedEmail = "ADMIN@GMAIL.COM",
                    FirstName = "System",
                    LastName = "Admin",
                    UserName = "admin@gmail.com",
                    NormalizedUserName = "ADMIN@GMAIL.COM",
                    PasswordHash = hasher.HashPassword(null, "Abcd@1234"),
                    EmailConfirmed = true
                },
                 new ApplicationUser
                 {
                     Id = "e9d21384-bf44-4098-b6be-e81892a2bf58",
                     Email = "user@gmail.com",
                     NormalizedEmail = "USER@GMAIL.COM",
                     FirstName = "System",
                     LastName = "User",
                     UserName = "user@gmail.com",
                     NormalizedUserName = "USER@GMAIL.COM",
                     PasswordHash = hasher.HashPassword(null, "Abcd@1234"),
                     EmailConfirmed = true
                 }
                );
        }
    }
}
