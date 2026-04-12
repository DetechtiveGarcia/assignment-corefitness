using Application.Abstractions.Services;
using Application.Dtos.FitnessClasses.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Attributes.MenuNavigation;
using Presentation.WebApp.Models.Forms.FitnessClasses;
using Presentation.WebApp.Models.ViewModels.FitnessClasses;

namespace Presentation.WebApp.Controllers;

public class ClassesController(IFitnessClassService fitnessClassService) : Controller
{
    [MenuItem(title: "Classes", order: 3)]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var result = await fitnessClassService.GetAllAsync();

        var vm = new FitnessClassesViewModel
        {
            Classes = result.FitnessClasses ?? [],
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

        if (!ModelState.IsValid)
        {
            var invalidVm = new FitnessClassesViewModel
            {
                Classes = classesResult.FitnessClasses ?? [],
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

            var failedVm = new FitnessClassesViewModel
            {
                Classes = classesResult.FitnessClasses ?? [],
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
}