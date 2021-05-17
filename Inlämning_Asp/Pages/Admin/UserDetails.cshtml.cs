using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inlämning_Asp.Data;
using Inlämning_Asp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Html;
using System.Text.Encodings.Web;

namespace Inlämning_Asp.Pages.Admin
{
    public class UserDetailsModel : PageModel
    {
        private readonly EventDbContext _context;
        private readonly UserManager<Buyer> _userManager;

        public UserDetailsModel(EventDbContext context, UserManager<Buyer> userManager)
        {
            _context = context;
            _userManager = userManager;           
        }

        [BindProperty]
        public Buyer ThisUser { get; set; }
        public IList<Event> UserEvents { get; set; }
        public IList<string> Roles { get; set; }
        public bool DetailsChanged { get; set; }

        public async Task OnGetAsync(string id, bool? detailsChanged)
        {
            if (id == null)
            {
                NotFound();
            }

            DetailsChanged = detailsChanged ?? false;

            // Hämta User
            ThisUser = await _context.Users
                .Include(a => a.Buyers)
                .FirstOrDefaultAsync(m => m.Id == id);

            // Hämta roll(er)
            Roles = await _userManager.GetRolesAsync(ThisUser);

            // Hämta Events som User är registrerad till
            UserEvents = ThisUser.OrganizedEvents;

            if (User == null)
            {
                NotFound();
            }
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(string id, bool? BanUser)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Hämta data för att skriva över
            var newUserDetails = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);

            // Skriv över gammal data med ny data
            newUserDetails.UserName = ThisUser.UserName;
            newUserDetails.FirstName = ThisUser.FirstName;
            newUserDetails.LastName = ThisUser.LastName;
            newUserDetails.Email = ThisUser.Email;
            newUserDetails.PhoneNumber = ThisUser.PhoneNumber;

            // Kolla om parametern ger ett BanUser = true eller false
            if (BanUser ?? false)
            {
                // Om Usern är utelåst så nollställs LockoutEnd
                if (newUserDetails.LockoutEnd != null)
                {
                    newUserDetails.LockoutEnd = null;
                }
                // Om Usern inte har något värde i LockoutEnd så läggs värde till
                else
                { 
                newUserDetails.LockoutEnd = DateTime.Now.AddDays(9999);
                newUserDetails.LockoutEnabled = true;
                }
            }

            // Försök att spara
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(ThisUser.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Använder RedirectToPage för att få till rätt resultat.
            // Skicka med bool när Save eller Ban  trycks för att få en alert samt id på User
            return RedirectToPage("./UserDetails", new { DetailsChanged = true,  id = id });
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
