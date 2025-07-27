namespace DelitaTrade.Infrastructure.Seeding.Interfaces
{
    public interface IIdentitySeeder
    {
        Task SeedApplicationRolesAsync();
        Task SeedApplicationUsersAsync();
    }
}
