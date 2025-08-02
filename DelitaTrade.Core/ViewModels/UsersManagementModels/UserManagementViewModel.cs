namespace DelitaTrade.Core.ViewModels.UsersManagementModels
{
    public class UserManagementViewModel
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public IEnumerable<string> Roles { get; set; } = new List<string>();
    }
}
