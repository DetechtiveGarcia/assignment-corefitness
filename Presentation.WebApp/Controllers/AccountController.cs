using Application.Abstractions.Identity;
using Application.Abstractions.Services;
using Application.Dtos.Members.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Attributes.MenuNavigation;
using Presentation.WebApp.Models.Forms.Members;
using System.Security.Claims;

namespace Presentation.WebApp.Controllers
{
    [Authorize]
    public class AccountController(IMemberService memberService, IIdentityService identityService) : Controller
    {
        [HideInMenu]
        public async Task<IActionResult> Me()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userId))
                return RedirectToAction("SignIn", "Authentication");

            var result = await memberService.GetMemberDetailAsync(userId);

            if (!result.Succeeded || result.MemberDetails is null)
            {
                TempData["ErrorMessage"] = string.Join(" ", result.Errors);
                return RedirectToAction("SignIn", "Authentication");
            }

            var form = new UpdateProfileForm
            {
                FirstName = result.MemberDetails.FirstName,
                LastName = result.MemberDetails.LastName,
                PhoneNumber = result.MemberDetails.PhoneNumber,
                ExistingProfileImageUrl = result.MemberDetails.ProfileImageUrl
            };

            ViewBag.Email = result.MemberDetails.Email;

            return View(form);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Me(UpdateProfileForm form)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userId))
                return RedirectToAction("SignIn", "Authentication");

            if (!ModelState.IsValid)
            {
                var email = await identityService.GetEmailAsync(userId);
                var memberServiceResult = await memberService.GetMemberDetailAsync(userId);

                ViewBag.Email = email;
                form.ExistingProfileImageUrl = memberServiceResult.MemberDetails?.ProfileImageUrl;

                return View(form);
            }

            var memberResult = await memberService.GetMemberDetailAsync(userId);
            if (!memberResult.Succeeded || memberResult.MemberDetails is null)
            {
                TempData["ErrorMessage"] = string.Join(" ", memberResult.Errors);
                return RedirectToAction("Me");
            }

            string? profileImageUrl = memberResult.MemberDetails.ProfileImageUrl;

            if (form.ProfileImage is not null && form.ProfileImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "profiles");
                Directory.CreateDirectory(uploadsFolder);

                var extension = Path.GetExtension(form.ProfileImage.FileName);
                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await form.ProfileImage.CopyToAsync(stream);

                profileImageUrl = $"/images/profiles/{fileName}";
            }

            var updateRequest = new UpdateMemberRequest(
                memberResult.MemberDetails.Id,
                form.FirstName ?? "",
                form.LastName ?? "",
                form.PhoneNumber,
                profileImageUrl
            );

            var result = await memberService.UpdateMemberAsync(updateRequest);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error);

                ViewBag.Email = memberResult.MemberDetails.Email;
                form.ExistingProfileImageUrl = profileImageUrl;

                return View(form);
            }

            TempData["SuccessMessage"] = "Profile updated successfully.";
            return RedirectToAction("Me");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccount()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("SignIn", "Authentication");

            var result = await memberService.DeleteMemberAsync(userId);

            if (!result.Succeeded)
            {
                TempData["ErrorMessage"] = string.Join(" ", result.Errors);
                return RedirectToAction("Me");
            }

            await identityService.LogoutAsync();

            return RedirectToAction("SignIn", "Authentication");
        }
    }
}
