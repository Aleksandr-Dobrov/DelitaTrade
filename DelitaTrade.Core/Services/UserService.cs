using Microsoft.EntityFrameworkCore;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Common;
using DelitaTrade.Infrastructure.Data.Models;
using DelitaTrade.Common;

namespace DelitaTrade.Core.Services
{
    public class UserService(IRepository repo) : IUserService
    {
        public async Task<UserViewModel> LogIn(UserValidationForm userLogin)
        {
            var user = await repo.AllReadonly<User>().FirstOrDefaultAsync(u => u.Name == userLogin.LoginName && u.Password == userLogin.Password);
            if (user == null) throw new ArgumentException($"User name: {userLogin.LoginName} or password is invalid");
            return new UserViewModel
            {
                Id = user.Id,
                Name = user.Name
            };
        }

        public async Task CreateUser(UserValidationForm userForm)
        {
            await repo.AddAsync(new User { Name = userForm.LoginName, Password = userForm.Password });
            await repo.SaveChangesAsync();
        }        
    }
}

