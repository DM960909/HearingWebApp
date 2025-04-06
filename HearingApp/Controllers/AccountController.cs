using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HearingApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using HearingApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace HearingApp.Controllers;


[Authorize]
public class AccountController : Controller
{

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;


    //Constructor for usermanager and signinmanger
    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public async Task<IActionResult> Settings()
    {
        var user = await _userManager.GetUserAsync(User);

        var model = new AccountSettingsViewModel
        {
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Settings(AccountSettingsViewModel model)
    {
        if(!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.GetUserAsync(User);

        user.Email = model.Email;
        user.UserName = model.Email;
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;

        var result = await _userManager.UpdateAsync(user);
        if(!result.Succeeded)
        {
            foreach(var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);

            }
            return View(model);
        }

        if(!string.IsNullOrEmpty(model.NewPassword))
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var passwordResult = await _userManager.ResetPasswordAsync(user,token,model.NewPassword);
            if(!passwordResult.Succeeded)
            {
                foreach(var error in passwordResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
        }

        await _signInManager.RefreshSignInAsync(user);
        ViewBag.StatusMessage = "Account Updated Successfully";
        return View(model);
    }
    //END POST TO UPDATE SETTINGS

    [HttpPost]
    public async Task<IActionResult> DeleteAccount()
    {
        var user = await _userManager.GetUserAsync(User); 

        if (user != null)
        {
            await _signInManager.SignOutAsync();           // Sign out user
            await _userManager.DeleteAsync(user);          // Delete account
        }

        return RedirectToAction("Index", "Home"); // Send back to homepage
    }


}
