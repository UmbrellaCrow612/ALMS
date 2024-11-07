namespace ALMS.API.Core.Constants
{
    public static class UserRoles
    {
        public const string Guest = "Guest";
        public const string BranchLibarian = "BranchLibarian";
        public const string LibaryMember = "LibaryMember";
        public const string CallCenterOperator = "CallCenterOperator";
        public const string Accountant = "Accountant";

        public static readonly string[] Roles = [Guest, BranchLibarian, LibaryMember, CallCenterOperator, Accountant];
    }
}
