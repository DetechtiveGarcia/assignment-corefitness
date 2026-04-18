using System.Security.Claims;
using Application.Abstractions.Services;
using Application.Dtos.FitnessClasses.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Attributes.MenuNavigation;
using Presentation.WebApp.Models.Forms.FitnessClasses;
using Presentation.WebApp.Models.ViewModels.FitnessClasses;

namespace Presentation.WebApp.Controllers;

public class ClassesController(
    IFitnessClassService fitnessClassService,
    IClassBookingService classBookingService) : Controller
{
    //[MenuItem(title: "Classes", order: 3)]
    [HideInMenu]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var classesResult = await fitnessClassService.GetAllAsync();

        IEnumerable<Application.Dtos.ClassBookings.ClassBookingDetails> myBookings = [];

        if (User.Identity?.IsAuthenticated == true)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrWhiteSpace(userId))
            {
                var bookingsResult = await classBookingService.GetMyBookingsAsync(userId);
                myBookings = bookingsResult.Bookings ?? [];
            }
        }

        var vm = new FitnessClassesPageViewModel
        {
            Classes = classesResult.FitnessClasses ?? [],
            MyBookings = myBookings,
            Form = new CreateFitnessClassForm()
        };

        return View(vm);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateFitnessClassForm form)
    {
        var classesResult = await fitnessClassService.GetAllAsync();

        IEnumerable<Application.Dtos.ClassBookings.ClassBookingDetails> myBookings = [];

        if (User.Identity?.IsAuthenticated == true)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrWhiteSpace(userId))
            {
                var bookingsResult = await classBookingService.GetMyBookingsAsync(userId);
                myBookings = bookingsResult.Bookings ?? [];
            }
        }

        if (!ModelState.IsValid)
        {
            var invalidVm = new FitnessClassesPageViewModel
            {
                Classes = classesResult.FitnessClasses ?? [],
                MyBookings = myBookings,
                Form = form
            };

            return View("Index", invalidVm);
        }

        var result = await fitnessClassService.CreateAsync(new CreateFitnessClassRequest(
            form.Title,
            form.Description,
            form.InstructorName,
            form.Category,
            form.StartTime,
            form.EndTime
        ));

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error);

            var failedVm = new FitnessClassesPageViewModel
            {
                Classes = classesResult.FitnessClasses ?? [],
                MyBookings = myBookings,
                Form = form
            };

            return View("Index", failedVm);
        }

        TempData["SuccessMessage"] = "Class created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return RedirectToAction(nameof(Index));

        await fitnessClassService.DeleteAsync(id);

        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Book(string id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(id))
            return RedirectToAction(nameof(Index));

        var result = await classBookingService.BookAsync(userId, id);

        TempData[result.Succeeded ? "SuccessMessage" : "ErrorMessage"] =
            result.Succeeded
                ? "Class booked successfully."
                : string.Join(" ", result.Errors);

        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CancelBooking(string id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(id))
            return RedirectToAction(nameof(Index));

        var result = await classBookingService.CancelAsync(userId, id);

        TempData[result.Succeeded ? "SuccessMessage" : "ErrorMessage"] =
            result.Succeeded
                ? "Booking cancelled successfully."
                : string.Join(" ", result.Errors);

        return RedirectToAction(nameof(Index));
    }
}