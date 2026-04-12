using Application.Abstractions.Identity;
using Application.Abstractions.Services;
using Application.Dtos.Members.Requests;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Attributes.MenuNavigation;
using Presentation.WebApp.Models.Forms.Authentication;

namespace Presentation.WebApp.Controllers;

public class AuthenticationController(IIdentityService identityService, IMemberService memberService) : Controller
{


    [HttpGet]
    public IActionResult SignIn(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [HideInMenu]
    [HttpGet("authentication/signup-email")]
    public IActionResult SignUpEmail()
    {
        return View();
    }

    [HideInMenu]
    [HttpGet]
    public IActionResult SignUpPassword()
    {
        var email = HttpContext.Session.GetString("Email");

        if (string.IsNullOrWhiteSpace(email))
            return RedirectToAction("SignUpEmail");

        return View();
    }
    [HttpPost("authentication/signup-email")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignUpEmail(SignUpEmailForm form)
    {
        if (!ModelState.IsValid)
        {
            return View(form);
        }

        var existing = await identityService.FindExistingAsync(form.Email);
        if (existing)
        {
            ModelState.AddModelError("", "An account with this email already exists.");
            return View(form);
        }

        HttpContext.Session.SetString("Email", form.Email);

        return RedirectToAction("SignUpPassword");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignUpPassword(SignUpPasswordForm form)
    {
        if (!form.AcceptTermsAndConditions)
            ModelState.AddModelError(nameof(form.AcceptTermsAndConditions), "You must accept the user terms and conditions.");
        if (!ModelState.IsValid)
            return View(form);

        var email = HttpContext.Session.GetString("Email");
        if (string.IsNullOrWhiteSpace(email))
        {
            TempData["ErrorMessage"] = "Your sign up session has expired.";
            return RedirectToAction("SignUpEmail");
        }

        var createdResult = await memberService.CreateMemberAsync(
            new RegisterMemberRequest(
                email,
                form.Password,
                null,
                null,
                null,
                null
            ));
        if (!createdResult.Succeeded)
        {
            foreach (var error in createdResult.Errors)
                ModelState.AddModelError("", error);

            return View(form);
        }

        var signedIn = await identityService.LoginAsync(email, form.Password, false);
        if (!signedIn)
        {
            TempData["ErrorMessage"] = "User was created, but automatic sign in failed.";
            return RedirectToAction("SignIn");
        }

        HttpContext.Session.Remove("Email");

        return RedirectToAction("Me", "Account");
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignIn(SignInForm form, string? returnUrl = null)
    {
        if (!ModelState.IsValid) return View(form);

        var user = await identityService.LoginAsync(form.Email, form.Password, form.RememberMe);
        if (!user)
        {
            ModelState.AddModelError("", "Incorrect email or password");
            ViewBag.ReturnUrl = returnUrl;
            return View(form);
        }
        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction("Me", "Account");


    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LogOut()
    {
        await identityService.LogoutAsync();
        return RedirectToAction("SignIn", "Authentication");
    }
}
