using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Inlämning_Asp.Data;
using Inlämning_Asp.Models;

namespace Inlämning_Asp.Pages
{
    public class MyEventsModel : PageModel
    {
        private readonly Inlämning_Asp.Data.EventDbContext _context;

        public MyEventsModel(Inlämning_Asp.Data.EventDbContext context)
        {
            _context = context;
        }

        public IList<Event> Events { get;set; }

        public async Task OnGetAsync()
        {
            var attendee = await _context.buyers.Include(a => a.Events).FirstOrDefaultAsync();
            Events = attendee.Events;
        }
    }
}
