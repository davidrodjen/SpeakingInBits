using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeakingInBits.Models
{
    public static class IdentityHelper
    {
        //Field that holds variable constants for roles, called in various places
        public const string Instructor = "Instructor";

        public const string Student = "Stu";

        public static void SetIdentityOptions(IdentityOptions options)
        {
            // Setting Sign in options, created a method and passed above
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;

            // Set password strength
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;

            // Set Lockout Options
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            options.Lockout.MaxFailedAccessAttempts = 5;
        }

        /// <summary>
        /// Params makes it process with individual identities that are added, creates roles
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public static async Task CreateRoles(IServiceProvider provider, params string[] roles)
        {
            RoleManager<IdentityRole> roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();

            // Create role if it does not exist
            foreach (string role in roles)
            {
                bool doesRoleExist = await roleManager.RoleExistsAsync(role);
                if (!doesRoleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        // Created by generating method from Startup, changed a few things
        public static async Task CreateDefaultInstructor(IServiceProvider serviceProvider)
        {
            const string email = "computerprogramming@cptc.edu";
            const string username = "instructor";
            const string password = "Programming";

            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // Check if any users are in database
            if(userManager.Users.Count() == 0)
            {
                IdentityUser instructor = new IdentityUser()
                {
                    Email = email,
                    UserName = username

                };

                // Create instructor
                await userManager.CreateAsync(instructor, password);

                // Add to instructor role
                await userManager.AddToRoleAsync(instructor, Instructor);
            }
        }
    }
}
