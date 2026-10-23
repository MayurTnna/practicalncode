namespace PracticalNCode.Application.Constants;

public static class StoredProcedures
{
    // Employee Stored Procedures (Consolidated CRUD & Get)
    public const string EmployeeManage = "dbo.sp_Employee_Manage";
    public const string EmployeeGet = "dbo.sp_Employee_Get";

    // Organization Stored Procedures (Consolidated CRUD & Get)
    public const string OrganizationManage = "dbo.sp_Organization_Manage";
    public const string OrganizationGet = "dbo.sp_Organization_Get";

    // User Stored Procedures (Consolidated CRUD & Get)
    public const string UserManage = "dbo.sp_User_Manage";
    public const string UserGet = "dbo.sp_User_Get";

    // AI Model Stored Procedures
    public const string AiModelGetActive = "dbo.sp_AiModel_GetActive";
}
