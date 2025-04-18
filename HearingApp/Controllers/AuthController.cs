using System;
using HearingApp.Data;
using HearingApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HearingApp.Controllers;

public class AuthController : Controller
{

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]

    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if(!ModelState.IsValid)
        {
            return View(model);
        }

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if(result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent:false);
            return RedirectToAction("Dashboard", "Tinnitus");
        }
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }


    // LOGIN AREA BELOW  
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    public async Task<IActionResult> Login(LoginViewModel model)
    {

        if(!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(
            model.Email, model.Password, model.RememberMe, lockoutOnFailure: false
        );

        if(result.Succeeded)
        {
            return RedirectToAction("Index", "Home"); //Index home until Dashboard is built. 
        }

        ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
        return View(model);

    }

    //END LOGIN AREA 

}
