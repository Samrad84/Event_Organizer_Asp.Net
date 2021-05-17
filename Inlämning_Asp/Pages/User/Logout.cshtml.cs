using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inlämning_Asp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Inlämning_Asp.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<Buyer> _signInManager;

        public LogoutModel(SignInManager<Buyer> signInManager)
        {
            _signInManager = signInManager;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            await _signInManager.SignOutAsync();

            return RedirectToPage("/Index");
        }
    }
}