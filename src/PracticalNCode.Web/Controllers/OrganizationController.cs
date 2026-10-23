using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PracticalNCode.Application.Abstractions.Services;
using PracticalNCode.Application.Models;

namespace PracticalNCode.Web.Controllers;

[Authorize(Roles = "Organization")]
public sealed class OrganizationController(IOrganizationService organizationService) : Controller
{
    private int? GetOrganizationId()
    {
        var claim = User.FindFirst("OrganizationId")?.Value;
        if (int.TryParse(claim, out var orgId) && orgId > 0)
        {
            return orgId;
        }
        return null;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
    {
        var orgId = GetOrganizationId();
        if (!orgId.HasValue)
        {
            return Forbid();
        }

        var employees = await organizationService.GetEmployeesByOrganizationAsync(orgId.Value, cancellationToken);
        return View(employees);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new CreateEmployeeViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateEmployeeViewModel model, CancellationToken cancellationToken = default)
    {
        var orgId = GetOrganizationId();
        if (!orgId.HasValue)
        {
            return Forbid();
        }

        model.OrganizationId = orgId.Value;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var (success, errorMessage, _) = await organizationService.CreateEmployeeAsync(model, cancellationToken);
        if (!success)
        {
            ModelState.AddModelError(string.Empty, errorMessage ?? "Failed to add employee.");
            return View(model);
        }

        TempData["SuccessMessage"] = $"Employee '{model.FullName}' ({model.EmployeeCode}) added successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken = default)
    {
        var orgId = GetOrganizationId();
        if (!orgId.HasValue)
        {
            return Forbid();
        }

        var employee = await organizationService.GetEmployeeForEditAsync(id, orgId.Value, cancellationToken);
        if (employee is null)
        {
            TempData["ErrorMessage"] = "Employee not found or access denied.";
            return RedirectToAction(nameof(Index));
        }

        return View(employee);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditEmployeeViewModel model, CancellationToken cancellationToken = default)
    {
        var orgId = GetOrganizationId();
        if (!orgId.HasValue)
        {
            return Forbid();
        }

        model.EmployeeId = id;
        model.OrganizationId = orgId.Value;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var (success, errorMessage) = await organizationService.UpdateEmployeeAsync(model, cancellationToken);
        if (!success)
        {
            ModelState.AddModelError(string.Empty, errorMessage ?? "Failed to update employee.");
            return View(model);
        }

        TempData["SuccessMessage"] = $"Employee '{model.FullName}' updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
    {
        var orgId = GetOrganizationId();
        if (!orgId.HasValue)
        {
            return Forbid();
        }

        var (success, errorMessage) = await organizationService.DeleteEmployeeAsync(id, orgId.Value, cancellationToken);
        if (!success)
        {
            TempData["ErrorMessage"] = errorMessage ?? "Failed to delete employee.";
        }
        else
        {
            TempData["SuccessMessage"] = "Employee deleted successfully. Linked login access has been deactivated.";
        }

        return RedirectToAction(nameof(Index));
    }
}

