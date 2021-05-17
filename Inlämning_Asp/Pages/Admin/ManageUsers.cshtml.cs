using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Inlämning_Asp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Inlämning_Asp.Models;

namespace Inlämning_Asp.Pages.Admin
{
    public class ManageUsersModel : PageModel
    {
        private readonly EventDbContext _context;
        private readonly UserManager<Buyer> _userManager;

        public ManageUsersModel(EventDbContext context, UserManager<Buyer> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Buyer> Users { get;set; }

        public async Task OnGetAsync()
        {
            Users = await _context.Users.ToListAsync();            
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return Page();
            }

            var organizerRole = "Organizer";
            var user = await _userManager.FindByIdAsync(id);

            if (await _userManager.IsInRoleAsync(user, organizerRole))
            {
                await _userManager.RemoveFromRoleAsync(user, organizerRole);
            }
            else
            {
                await _userManager.AddToRoleAsync(user, organizerRole);
            }

            return RedirectToPage();
        }

        public async Task<bool> IsOrganizer(string id)
        {
            IdentityRole role = await _context.Roles
                .Where(r => r.Name == "Organizer")
                .FirstOrDefaultAsync();

            return await _context.UserRoles
                .AnyAsync(u => u.UserId == id && u.RoleId == role.Id);    
        }
    }
}
