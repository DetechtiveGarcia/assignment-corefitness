using System.Security.Claims;
using Application.Abstractions.Services;
using Application.Dtos.Memberships.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Attributes.MenuNavigation;
using Presentation.WebApp.Models.Forms.Memberships;

namespace Presentation.WebApp.Controllers;


public class MembershipsController(IMembershipService membershipService) : Controller
{
    [MenuItem(title: "Memberships", order: 2)]
    [HttpGet]
    public IActionResult Index()
    {
        return View(new CreateMembershipForm());
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(CreateMembershipForm form)
    {
        if (!ModelState.IsValid)
            return View(form);

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId))
            return RedirectToAction("SignIn", "Authentication");

        var created = await membershipService.CreateMembershipAsync(
            new CreateMembershipRequest(userId, form.MembershipName));

        if (!created)
        {
            ModelState.AddModelError("", "Unable to create membership. You may already have one.");
            return View(form);
        }

        TempData["SuccessMessage"] = "Membership created successfully.";
        return RedirectToAction("Me", "Account");
    }
}