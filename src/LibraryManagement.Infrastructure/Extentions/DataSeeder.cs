using LibraryManagement.Core.Entities;
using LibraryManagement.Infrastructure.Constants;
using LibraryManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace LibraryManagement.Infrastructure.Extentions
{
    public static class DataSeeder
    {
        public static void SeedData(IServiceProvider serviceProvider)
        {
            using (var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>())
            {
                if (!roleManager.Roles.Any())
                {
                    string[] roleNames = { RoleName.Librarian, RoleName.Customer };

                    foreach (var roleName in roleNames)
                    {
                        // Create the role
                        var role = new IdentityRole(roleName);
                        roleManager.CreateAsync(role).Wait();
                    }
                }
            }
           
            using (var userManager = serviceProvider.GetRequiredService<UserManager<User>>())
            {
                if (!userManager.Users.Any())
                {
                    User admin = new User()
                    {
                        FirstName = "Admin",
                        LastName = "Library",
                        UserName = "AdminLibrary1@gmail.com",
                        Email = "AdminLibrary1@gmail.com",
                        IsActivated = true,
                    };
                   var result = userManager.CreateAsync(admin, "LibraryAdmin#1").Result;
                   userManager.AddToRoleAsync(admin, RoleName.Librarian).Wait();
                }
            }

            using (var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>())
            {
                if (!dbContext.Categories.Any())
                {
                    dbContext.Categories.AddRange(
                        new Category()
                        {
                            CategoryId = Guid.NewGuid().ToString(),
                            CategoryName = "Novel"
                        },
                        new Category()
                        {
                            CategoryId = Guid.NewGuid().ToString(),
                            CategoryName = "Fiction"
                        },
                        new Category()
                        {
                            CategoryId = Guid.NewGuid().ToString(),
                            CategoryName = "Biography"
                        },
                        new Category()
                        {
                            CategoryId = Guid.NewGuid().ToString(),
                            CategoryName = "Psychology"
                        },
                        new Category()
                        {
                            CategoryId = Guid.NewGuid().ToString(),
                            CategoryName = "Business & Economics"
                        }
                    );
                    dbContext.SaveChanges();
                }

                if (!dbContext.Authors.Any())
                {
                    dbContext.Authors.AddRange(
                        new Author()
                        {
                            AuthorId = Guid.NewGuid().ToString(),
                            AuthorName = "Jasmine Cruz"
                        },
                        new Author()
                        {
                            AuthorId = Guid.NewGuid().ToString(),
                            AuthorName = "Liam Patel"
                        },
                        new Author()
                        {
                            AuthorId = Guid.NewGuid().ToString(),
                            AuthorName = "Isabella Nguyen"
                        },
                        new Author()
                        {
                            AuthorId = Guid.NewGuid().ToString(),
                            AuthorName = "Noah Khan"
                        }
                    );
                    dbContext.SaveChanges();
                }
            }
        }
    }
}
