using System.Security.Claims;
using Application.Abstractions.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Attributes.MenuNavigation;

namespace Presentation.WebApp.Controllers;

[Authorize]
public class BookingsController(IClassBookingService classBookingService) : Controller
{
    //[MenuItem(title: "My Bookings", order: 4)]
    [HideInMenu]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userId))
            return RedirectToAction("SignIn", "Authentication");

        var result = await classBookingService.GetMyBookingsAsync(userId);

        if (!result.Succeeded)
        {
            TempData["ErrorMessage"] = string.Join(" ", result.Errors);
            return View(Enumerable.Empty<Application.Dtos.ClassBookings.ClassBookingDetails>());
        }

        return View(result.Bookings ?? []);
    }
}