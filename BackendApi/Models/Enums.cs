namespace TaskManagementAPI.Models
{
    // Re‑export enums for DTOs.
    public enum TaskStatus
    {
        InProgress,
        AwaitingReview,
        NeedsWork,
        Completed
    }

    // Defines the possible roles for a User.
    public enum UserRole
    {
        Admin,
        User
    }
}