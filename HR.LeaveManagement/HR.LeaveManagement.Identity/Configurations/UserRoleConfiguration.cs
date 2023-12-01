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
    public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "320dabad-866b-4eed-a6fb-394f7a58b69b",
                    UserId = "e03da57b-54f0-4276-98b9-95d08a6d4749"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "f24d6dde-a027-43da-ae7e-9896b61c9134",
                    UserId = "e9d21384-bf44-4098-b6be-e81892a2bf58"
                }
            );
        }
    }
}
