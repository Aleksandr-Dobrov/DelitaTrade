using DelitaTrade.Common;
using DelitaTrade.WPFCore.Contracts;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace DelitaTrade.WPFCore.Services
{
    public class UserService(UserManager<DelitaUser> userManager, IUserStore<DelitaUser> userStore) : IUserService
    {
        public async Task<UserViewModel> LogIn(UserValidationForm userLogin)
        {
            if (userManager == null) throw new ArgumentNullException(nameof(userManager), "User manager is not initialized");

            var loginUser = await userManager.FindByNameAsync(userLogin.LoginName.ToUpper()) ?? throw new ArgumentException($"User name: {userLogin.LoginName} or password is invalid");

            var result = userManager.PasswordHasher.VerifyHashedPassword(loginUser, loginUser.PasswordHash, userLogin.Password);

            if (result == PasswordVerificationResult.Success)
            {
                return new UserViewModel
                {
                    Id = loginUser.Id,
                    Name = $"{loginUser.Name} {loginUser.LastName}",
                    UserName = loginUser.UserName ?? throw new ArgumentException("User name is null")
                };
            }
            else
            {
                throw new ArgumentException($"User name: {userLogin.LoginName} or password is invalid");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userForm"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task CreateUser(UserValidationForm userForm)
        {
            if (userManager == null) throw new ArgumentNullException(nameof(userManager), "User manager is not initialized");

            var newUser = new DelitaUser()
            {
                Name = userForm.FirstName ?? throw new ArgumentException("First name is required"),
                LastName = userForm.LastName ?? throw new ArgumentException("Last name is required")
            };
            await userStore.SetUserNameAsync(newUser, userForm.LoginName, CancellationToken.None);

            var identityResult = await userManager.CreateAsync(newUser, userForm.Password);

            if (identityResult.Succeeded == false)
            {
                throw new ArgumentException($"User creation failed: {string.Join(", ", identityResult.Errors.Select(e => e.Description))}");
            }
        }

        public async Task<bool> IsUserExist(UserValidationForm userLogin)
        {
            if (userStore == null) throw new ArgumentNullException(nameof(userManager), "User store is not initialized");

            if (await userStore.FindByNameAsync(userLogin.LoginName.ToUpper(), CancellationToken.None) == null)
            {
                return false;
            }
            return true;
        }
    }
}
