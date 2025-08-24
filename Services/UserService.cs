using Microsoft.AspNetCore.Identity;
using projekatPPP.Models;
using projekatPPP.Models.ViewModels; // Dodaj ovaj using
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projekatPPP.Services
{
    public class UserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IEnumerable<ApplicationUser> GetAllUsers()
        {
            return _userManager.Users.ToList();
        }

        public async Task<ApplicationUser?> GetUser(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<IdentityResult> CreateUser(ApplicationUser user, string password, string role)
        {
            user.UserName = user.Email;
            user.IsApproved = true;
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, role);
            }
            return result;
        }

        public async Task<IdentityResult> UpdateUser(EditUserViewModel model)
        {
            var existingUser = await _userManager.FindByIdAsync(model.Id);
            if (existingUser == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Korisnik nije pronađen." });
            }
            
            existingUser.Ime = model.Ime;
            existingUser.Prezime = model.Prezime;
            existingUser.Email = model.Email;
            existingUser.UserName = model.Email;
            existingUser.IsApproved = model.IsApproved;
            existingUser.OdeljenjeId = model.OdeljenjeId; // Sačuvaj promenu odeljenja
            
            return await _userManager.UpdateAsync(existingUser);
        }

        public async Task<IdentityResult> ApproveUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Korisnik nije pronađen." });
            }

            user.IsApproved = true;
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                return await _userManager.DeleteAsync(user);
            }
            return IdentityResult.Failed(new IdentityError { Description = "Korisnik nije pronađen." });
        }
    }
}