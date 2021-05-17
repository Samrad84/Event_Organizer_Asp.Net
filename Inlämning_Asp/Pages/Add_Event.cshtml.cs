using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Inlämning_Asp.Data;
using Inlämning_Asp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Ical.Net.DataTypes;
using Organizer = Inlämning_Asp.Models.Organizer;

namespace Inlämning_Asp.Pages
{
    

    public class Add_EventModel : PageModel
    {
        private readonly Inlämning_Asp.Data.EventDbContext _context;

        public Add_EventModel(Inlämning_Asp.Data.EventDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Event Event { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Events.Add(Event);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
