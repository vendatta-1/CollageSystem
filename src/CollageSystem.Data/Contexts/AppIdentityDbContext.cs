using CollageSystem.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CollageSystem.Data.Contexts;

public class AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : IdentityDbContext<AppUser>(options)

{
    //protected override void OnModelCreating(ModelBuilder builder)
    //{
    //    var roles = new IdentityRole[]
    //    {
    //        new IdentityRole()
    //        {
    //            Name = "admin",
    //            NormalizedName = "ADMIN",
    //            Id = Guid.NewGuid().ToString()
    //        },
    //        new IdentityRole()
    //        {
    //            Name = "user",
    //            NormalizedName = "USER",
    //            Id = Guid.NewGuid().ToString()
    //        }
    //    };
    //    builder.Entity<IdentityRole>().HasData(roles);
    //    base.OnModelCreating(builder);
    //}
}