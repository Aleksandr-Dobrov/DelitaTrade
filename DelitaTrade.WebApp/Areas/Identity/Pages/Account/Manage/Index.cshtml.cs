// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DelitaTrade.Common;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DelitaTrade.WebApp.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<DelitaUser> _userManager;
        private readonly SignInManager<DelitaUser> _signInManager;

        public IndexModel(
            UserManager<DelitaUser> userManager,
            SignInManager<DelitaUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [MaxLength(ValidationConstants.DelitaUserConstants.NameMaxLength)]
            [MinLength(ValidationConstants.DelitaUserConstants.NamesMinLength)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [MaxLength(ValidationConstants.DelitaUserConstants.LastNameMaxLength)]
            [MinLength(ValidationConstants.DelitaUserConstants.NamesMinLength)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }
        }

        private async Task LoadAsync(DelitaUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var firstName = user.Name;
            var lastName = user.LastName;

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FirstName = firstName,
                LastName = lastName
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (ModelState.IsValid == false)
            {
                await LoadAsync(user);
                return Page();
            }
            string statusMessage = "Unexpected error when trying to set ";
            IdentityResult setPhoneResult = null;
            IdentityResult setFirstNameResult = null;
            IdentityResult setLastNameResult = null;

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (setPhoneResult.Succeeded == false)
                {
                    statusMessage += "phone number,";
                }
            }

            if (Input.FirstName != user.Name)
            {
                user.Name = Input.FirstName;
                setFirstNameResult = await _userManager.UpdateAsync(user);
                if (setFirstNameResult.Succeeded == false)
                {
                    statusMessage += "first name,";
                }
            }

            if (Input.LastName != user.LastName)
            {
                user.LastName = Input.LastName;
                setLastNameResult = await _userManager.UpdateAsync(user);
                if (setLastNameResult.Succeeded == false)
                {
                    statusMessage += "last name.";
                }
            }

            if ((setPhoneResult != null && setPhoneResult.Succeeded == false) ||
                (setFirstNameResult != null && setFirstNameResult.Succeeded == false) ||
                (setLastNameResult != null && setLastNameResult.Succeeded == false))
            {
                StatusMessage = $"{statusMessage[..^1]}.";
                return RedirectToPage();
            }


            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
