using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Common;
using DelitaTrade.Infrastructure.Data.Models;
using DelitaTrade.Common;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace DelitaTrade.Core.Services
{
    public class UserService(IRepository repo) : IUserService
    {
        PasswordHasher<string> passwordHasher = new PasswordHasher<string>();
        public async Task<UserViewModel> LogIn(UserValidationForm userLogin)
        {            
            var user = await repo.AllReadonly<User>().FirstOrDefaultAsync(u => u.Name == userLogin.LoginName);
            if (user == null) throw new ArgumentException($"User name: {userLogin.LoginName} or password is invalid");
            var result = passwordHasher.VerifyHashedPassword(userLogin.LoginName, user.HashedPassword, userLogin.Password);
            if(result == PasswordVerificationResult.Failed) throw new ArgumentException($"User name: {userLogin.LoginName} or password is invalid");
            if(result == PasswordVerificationResult.SuccessRehashNeeded)
            {                
                user.HashedPassword = passwordHasher.HashPassword(user.Name, userLogin.Password);
                await repo.SaveChangesAsync();
            }
            return new UserViewModel
            {
                Id = user.Id,
                Name = user.Name
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userForm"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task CreateUser(UserValidationForm userForm)
        {
            if(await repo.AllReadonly<User>().FirstOrDefaultAsync(u => u.Name == userForm.LoginName) != null) throw new ArgumentException($"User name: {userForm.LoginName} already exists");
            var newUser = new User { Name = userForm.LoginName, HashedPassword = passwordHasher.HashPassword(userForm.LoginName, userForm.Password) };
            await repo.AddAsync(newUser);
            await repo.SaveChangesAsync();
        }        
    }
}

