using GiftOfTheGivers.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Web.Data
{
    // Runs once at startup. Creates the two roles the POE asks for (Employee, Donor),
    // seeds one demo user per role, and drops in a few sample relief projects so the
    // Employee Dashboard has real data to display instead of an empty screen.
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            await context.Database.EnsureCreatedAsync();

            // --- Roles ---
            string[] roles = { "Employee", "Donor" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // --- Demo users (credentials match the Figma prototype so markers/lecturers can log straight in) ---
            await EnsureUserAsync(userManager, "employee@gotg.org", "Employee123!", "Thandi Nkosi", "Employee");
            await EnsureUserAsync(userManager, "donor@gotg.org", "Donor123!", "Reggie Bagg", "Donor");

            // --- Sample relief projects & field updates, so the dashboard isn't empty on first run ---
            if (!context.ReliefProjects.Any())
            {
                var projects = new List<ReliefProject>
                {
                    new() { Name = "Gaza Medical Aid", IsActive = true },
                    new() { Name = "Eastern Cape Flood Relief", IsActive = true },
                    new() { Name = "KZN Drought Response", IsActive = true },
                };
                context.ReliefProjects.AddRange(projects);
                await context.SaveChangesAsync();

                context.FieldUpdates.AddRange(
                    new FieldUpdate
                    {
                        ReliefProjectId = projects[0].Id,
                        ProjectName = projects[0].Name,
                        Message = "Third consignment of surgical supplies cleared customs in Cairo and is en route to Rafah. 4,200 trauma kits included.",
                        PostedBy = "Imtiaz Sooliman",
                        PostedAt = DateTime.UtcNow.AddDays(-15)
                    },
                    new FieldUpdate
                    {
                        ReliefProjectId = projects[1].Id,
                        ProjectName = projects[1].Name,
                        Message = "Day 18: 1,850 food parcels distributed in Butterworth. Mobile clinic saw 312 patients. Water tankers operational.",
                        PostedBy = "Thandi Nkosi",
                        PostedAt = DateTime.UtcNow.AddDays(-17)
                    },
                    new FieldUpdate
                    {
                        ReliefProjectId = projects[2].Id,
                        ProjectName = projects[2].Name,
                        Message = "Six boreholes commissioned in Msinga district. Water yield confirmed at 4,200 L/hour. Community training underway.",
                        PostedBy = "Ahmed Patel",
                        PostedAt = DateTime.UtcNow.AddDays(-19)
                    }
                );
                await context.SaveChangesAsync();
            }

            // --- A couple of sample donations so "Donations This Month" isn't zero ---
            if (!context.Donations.Any())
            {
                context.Donations.AddRange(
                    new Donation
                    {
                        DonationType = "One-Time",
                        Currency = "ZAR",
                        Amount = 500000,
                        FullName = "Corporate Partner Trust",
                        Email = "giving@partnertrust.co.za",
                        CertificateNumber = GenerateCertificateNumber(),
                        DonatedAt = DateTime.UtcNow.AddDays(-6)
                    },
                    new Donation
                    {
                        DonationType = "Recurring",
                        Currency = "ZAR",
                        Amount = 700000,
                        FullName = "Anonymous Foundation",
                        Email = "contact@anonfoundation.org",
                        CertificateNumber = GenerateCertificateNumber(),
                        DonatedAt = DateTime.UtcNow.AddDays(-3)
                    }
                );
                await context.SaveChangesAsync();
            }
        }

        private static async Task EnsureUserAsync(UserManager<ApplicationUser> userManager, string email, string password, string fullName, string role)
        {
            var existing = await userManager.FindByEmailAsync(email);
            if (existing != null) return;

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FullName = fullName,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, role);
            }
        }

        public static string GenerateCertificateNumber()
        {
            return "ABC" + Random.Shared.Next(10000, 99999);
        }
    }
}
