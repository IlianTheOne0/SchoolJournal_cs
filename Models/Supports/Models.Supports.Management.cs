namespace Models.Supports.Management;

public enum ManagementState
{
    NoneSelected,
    AddNewEdu,
}

public record ManagementRule(
    Func<int, int, int, bool> Condition,
    ManagementState State
);