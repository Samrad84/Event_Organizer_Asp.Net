using Inlämning_Asp.Data;
using Inlämning_Asp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inlämning_Asp.Pages
{
    [Authorize(Roles = "Organizer")]
    public class OrganizerEventsModel : PageModel
    {
        private readonly EventDbContext _context;
        private readonly UserManager<Buyer> _userManager;

        public OrganizerEventsModel(EventDbContext context, UserManager<Buyer> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public ICollection<Event> Event { get; set; }

        public async Task OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _context.Users
                .Where(u => u.Id == userId)
                .Include(u => u.HostedEvents)
                .FirstOrDefaultAsync();

            Event = user.HostedEvents;
        }
    }
}