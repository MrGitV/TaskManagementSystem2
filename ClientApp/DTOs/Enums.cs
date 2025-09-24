namespace ClientApp.DTOs
{
    // Defines the possible statuses for a task.
    public enum TaskStatus
    {
        InProgress,
        AwaitingReview,
        NeedsWork,
        Completed
    }

    // Defines the possible roles for a user.
    public enum UserRole
    {
        Admin,
        User
    }
}