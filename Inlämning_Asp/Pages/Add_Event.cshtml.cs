using Inlämning_Asp.Data;
using Inlämning_Asp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Inlämning_Asp.Pages
{
    [Authorize(Roles = "Organizer")]
    public class Add_EventModel : PageModel
    {
        private readonly EventDbContext _context;
        private readonly UserManager<Buyer> _userManager;

        public Add_EventModel(EventDbContext context, UserManager<Buyer> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Event Event { get; set; }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var userId = _userManager.GetUserId(User);
            var user = _context.Users
                .Where(u => u.Id == userId)
                .Include(u => u.HostedEvents)
                .FirstOrDefaultAsync();

            Event.Organizer = user.Result;

            _context.Events.Add(Event);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}