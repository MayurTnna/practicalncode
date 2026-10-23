namespace PracticalNCode.Application.Constants;

public static class EnumLookup
{
    public enum UserRole
    {
        Admin = 1,
        Organization = 2,
        Employee = 3
    }

    public static class DbAction
    {
        public const string Insert = "I";
        public const string Update = "U";
        public const string Delete = "D";
        public const string List = "L";
        public const string Single = "S";
        public const string ByOrgAndId = "B";
        public const string All = "A";
        public const string WithoutLogin = "W";
        public const string ByUsername = "U";
        public const string Exists = "E";
    }
}
