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
    public class Delete_eventModel : PageModel
    {
        private readonly Inlämning_Asp.Data.EventDbContext _context;

        public Delete_eventModel(Inlämning_Asp.Data.EventDbContext context)
        {
            _context = context;
        }

        public Event Event { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Event = await _context.Events.FirstOrDefaultAsync(m => m.Id == id);

            if (Event == null)
            {
                return NotFound();
            }
            return Page();
        }
        //public async Task<IActionResult> OnPostAsync(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    Event = await _context.Events.Include(e => e.Events).FirstOrDefaultAsync(m => m.Id == id);

        //    if (Event == null)
        //    {
        //        return NotFound();
        //    }

        //    var events = await _context.Events.FirstOrDefaultAsync();

        //    if (!Event.Events.Contains(events))
        //    {
        //        Event.Events.Remove(events);
        //        await _context.SaveChangesAsync();
        //    }



        //return Page();


        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return Page();
            }

            _context.Events.Remove(Event);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
