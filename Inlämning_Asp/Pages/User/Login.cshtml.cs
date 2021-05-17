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
    public class LoginModel : PageModel
    {
        private readonly SignInManager<Buyer> _signInManager;
        private readonly UserManager<Buyer> _userManager;

        public LoginModel(SignInManager<Buyer> signInManager,
            UserManager<Buyer> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [BindProperty]
        public LoginUserForm LoginUser { get; set; }
        public class LoginUserForm
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }
        public async Task<IActionResult> OnPost()
        {
            var result = await _signInManager.PasswordSignInAsync(LoginUser.UserName, LoginUser.Password, false, false);

            if (result.Succeeded)
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }
    }
}