namespace Models.Supports.Management;

public enum ManagementState
{
    NoneSelected,
    AddNewEdu,
    ExistingEdu,
    AddNewClass
}

public record ManagementRule(
    Func<int, int, int, bool> Condition,
    ManagementState State
);