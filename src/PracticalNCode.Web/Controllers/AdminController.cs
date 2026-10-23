using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PracticalNCode.Application.Abstractions.Services;
using PracticalNCode.Application.Models;

namespace PracticalNCode.Web.Controllers;

[Authorize(Roles = "Admin")]
public sealed class AdminController(IAdminService adminService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
    {
            var organizations = await adminService.GetAllOrganizationsAsync(cancellationToken);
        var users = await adminService.GetAllUsersAsync(cancellationToken);
        var employees = await adminService.GetAllEmployeesAsync(cancellationToken);

        ViewBag.TotalOrganizations = organizations.Count;
        ViewBag.TotalEmployees = employees.Count;
        ViewBag.TotalUsers = users.Count;
        ViewBag.Users = users;

        return View(organizations);
    }

    [HttpGet]
    public IActionResult CreateOrganization()
    {
        return View(new CreateOrganizationViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateOrganization(CreateOrganizationViewModel model, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var (success, errorMessage, _) = await adminService.CreateOrganizationWithLoginAsync(model, cancellationToken);
        if (!success)
        {
            ModelState.AddModelError(string.Empty, errorMessage ?? "Failed to create organization.");
            return View(model);
        }

        TempData["SuccessMessage"] = $"Organization '{model.OrganizationName}' and login account '{model.Username}' created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> EmployeeLogins(CancellationToken cancellationToken = default)
    {
        var employees = await adminService.GetAllEmployeesAsync(cancellationToken);
        return View(employees);
    }

    [HttpGet]
    public async Task<IActionResult> CreateEmployeeLogin(int? employeeId = null, CancellationToken cancellationToken = default)
    {
        var employeesWithoutLogin = await adminService.GetEmployeesWithoutLoginAsync(cancellationToken);

        if (employeeId.HasValue && employeeId.Value > 0)
        {
            var model = await adminService.GetEmployeeForLoginCreationAsync(employeeId.Value, cancellationToken);
            if (model is null)
            {
                TempData["ErrorMessage"] = "Selected employee not found, deleted, or already has a login account.";
                return RedirectToAction(nameof(EmployeeLogins));
            }

            ViewBag.EmployeeList = new SelectList(
                employeesWithoutLogin,
                nameof(EmployeeItemViewModel.EmployeeId),
                nameof(EmployeeItemViewModel.FullName),
                model.EmployeeId);

            return View(model);
        }

        if (employeesWithoutLogin.Count == 0)
        {
            TempData["InfoMessage"] = "All existing active employees already have login accounts linked.";
            return RedirectToAction(nameof(EmployeeLogins));
        }

        ViewBag.EmployeeList = new SelectList(
            employeesWithoutLogin.Select(e => new
            {
                e.EmployeeId,
                DisplayName = $"{e.FullName} ({e.EmployeeCode}) - {e.OrganizationName}"
            }),
            "EmployeeId",
            "DisplayName");

        return View(new CreateEmployeeLoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateEmployeeLogin(CreateEmployeeLoginViewModel model, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            var employeesWithoutLogin = await adminService.GetEmployeesWithoutLoginAsync(cancellationToken);
            ViewBag.EmployeeList = new SelectList(
                employeesWithoutLogin.Select(e => new
                {
                    e.EmployeeId,
                    DisplayName = $"{e.FullName} ({e.EmployeeCode}) - {e.OrganizationName}"
                }),
                "EmployeeId",
                "DisplayName",
                model.EmployeeId);

            return View(model);
        }

        var (success, errorMessage, _) = await adminService.CreateEmployeeLoginAsync(model, cancellationToken);
        if (!success)
        {
            ModelState.AddModelError(string.Empty, errorMessage ?? "Failed to create employee login.");
            var employeesWithoutLogin = await adminService.GetEmployeesWithoutLoginAsync(cancellationToken);
            ViewBag.EmployeeList = new SelectList(
                employeesWithoutLogin.Select(e => new
                {
                    e.EmployeeId,
                    DisplayName = $"{e.FullName} ({e.EmployeeCode}) - {e.OrganizationName}"
                }),
                "EmployeeId",
                "DisplayName",
                model.EmployeeId);

            return View(model);
        }

        TempData["SuccessMessage"] = $"Employee login account '{model.Username}' successfully created and linked.";
        return RedirectToAction(nameof(EmployeeLogins));
    }
}

