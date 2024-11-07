namespace ALMS.API.Core.Constants
{
    public static class UserRoles
    {
        public static readonly string Guest = "Guest";
        public static readonly string BranchLibarian = "BranchLibarian";
        public static readonly string LibaryMember = "LibaryMember";
        public static readonly string CallCenterOperator = "CallCenterOperator";
        public static readonly string Accountant = "Accountant";

        public static readonly string[] Roles = [Guest, BranchLibarian, LibaryMember, CallCenterOperator, Accountant];
    }
}
