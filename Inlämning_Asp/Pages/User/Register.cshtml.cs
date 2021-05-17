using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inlämning_Asp.Data;
using Inlämning_Asp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Inlämning_Asp.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<Buyer> _userManager;

        public RegisterModel(UserManager<Buyer> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public NewUserForm NewUser { get; set; }
        public class NewUserForm
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }
        public async Task<IActionResult> OnPost()
        {
           Buyer newUser = new()
            {
                UserName = NewUser.UserName,
            };

            var result = await _userManager.CreateAsync(newUser, NewUser.Password);

            if (result.Succeeded)
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }
    }
}