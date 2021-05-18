using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Inlämning_Asp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Inlämning_Asp.Data
{
    public class EventDbContext : IdentityDbContext<Buyer>
    {
        public EventDbContext(DbContextOptions<EventDbContext> options)
            : base(options)
        {
        }
        public DbSet<Event> Events { get; set; }
        public DbSet<Organizer> Organizers { get; set; }
        public DbSet<Buyer> buyers { get; set; }
     


        async Task Check(Task<IdentityResult> result)
        {
            if (!(await result).Succeeded)
            {
                throw new Exception();
            }
        }

        public async Task ResetAndSeedAsync(
            UserManager<Buyer> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            await Database.EnsureDeletedAsync();
            await Database.EnsureCreatedAsync();

            await Check(roleManager.CreateAsync(new IdentityRole("Attendee")));
            await Check(roleManager.CreateAsync(new IdentityRole("Admin")));
            await Check(roleManager.CreateAsync(new IdentityRole("Organizer")));

            Buyer admin = new Buyer()
            {
                UserName = "admin",
                Email = "admin@hotmail.com",
            };
            await userManager.CreateAsync(admin, "Passw0rd!");
            await Check(userManager.AddToRoleAsync(admin, "Admin"));

            Buyer superadmin = new Buyer()
            {
                UserName = "superadmin",
                Email = "superadmin@hotmail.com",
            };
            await userManager.CreateAsync(superadmin, "Passw0rd!");
            await Check(userManager.AddToRoleAsync(superadmin, "Admin"));

           Buyer user = new Buyer()
            {
                UserName = "test_user",
                Email = "test@hotmail.com",
            };
            await Check(userManager.CreateAsync(user, "Passw0rd!"));

            Buyer[] organizers = new Buyer[] {
                new Buyer(){
                    FirstName = "Arvin",
                    UserName = "Funcorp",
                    Email = "info@funcorp.com",
                    PhoneNumber = "+1 203 43 234",
                },
                new Buyer(){
                    UserName = "Funcorp1",
                    Email = "info1@funcorp.com",
                    PhoneNumber = "+1 203 43 234",
                },
                new Buyer(){
                    UserName = "Funcorp2",
                    Email = "info2@funcorp.com",
                    PhoneNumber = "+1 203 43 234",
                },
            };
            foreach (var org in organizers)
            {
                await Check(userManager.CreateAsync(org, "Passw0rd!"));
                await Check(userManager.AddToRoleAsync(org, "Organizer"));
            }

            Event[] events = new Event[] {
                new Event(){
                    Title="Summer camp",
                    Description="Have a fun time chilling in the sun",
                    Place="Colorado springs",
                    Address="515 S Cascade Ave Colorado Springs, CO 80903",
                    Date=DateTime.Now.AddDays(34),
                    SpotsAvailable=234,
                    Organizer= organizers[0],
                },
                new Event(){
                    Title="Moonhaven",
                    Description="Best lazertag in the world",
                    Place="Blackpark",
                    Address="510 N McPherson Church Rd Fayetteville, NC 28303",
                    Date=DateTime.Now.AddDays(12),
                    SpotsAvailable=23,
                    Organizer= organizers[1],
                },
            };

            await AddRangeAsync(events);

            await SaveChangesAsync();
        }
    }
}